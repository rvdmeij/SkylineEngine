using System.Collections.Generic;
using Assimp;
using SkylineEngine.Utilities;

namespace SkylineEngine
{



    public static class ModelImporter
    {

        public class Node
        {
            public Node parent;
            public string name;
            public Matrix4x4 transformation;
            public List<MMesh> meshes;
            public List<Node> nodes;

            public Node(Node parent)
            {
                this.parent = parent;
            }
        }


        public struct MaterialInfo
        {
            public string name;
            public Vector3 ambient;
            public Vector3 diffuse;
            public Vector3 specular;
            public float shininess;
        }

        public class MMesh
        {
            public string name;
            public int indexOffset;
            public int indexCount;
            public MaterialInfo material;
            public List<Vertex> vertices;
            public List<uint> indices;
        }


        private static List<MMesh> m_meshes = new List<MMesh>();
        private static List<Vector3> m_vertices = new List<Vector3>();
        private static List<Vector3> m_normals = new List<Vector3>();
        private static List<Vector2> m_uvs = new List<Vector2>();
        private static List<int> m_indices = new List<int>();
        private static Dictionary<int, MaterialInfo> m_materials = new Dictionary<int, MaterialInfo>();
        //private static Node m_rootNode;

        public static GameObject LoadFromFile(string filepath, List<string> shaders = null)
        {
            string directory = System.IO.Path.GetDirectoryName(filepath);
            Debug.Log(directory);

            AssimpContext importer = new AssimpContext();
            var scene = importer.ImportFile(filepath, PostProcessPreset.TargetRealTimeMaximumQuality);

            var components = filepath.Split('.');

            string name = "ImportedModel";

            if(components.Length > 1)
            {
                name = components[components.Length - 1];
            }

            string shaderPath = "res/Shaders/Default.shader";

            if(shaders?.Count > 0)
                shaderPath = shaders[0];

            GameObject parent = new GameObject(name);
            var shader = Resources.Load<Shader>(shaderPath);
            var texture = Resources.Load<Texture>("res/Textures/Default.png");

            var materials = scene.Materials;

            for (int i = 0; i < scene.Meshes.Count; i++)
            {
                GameObject child = new GameObject(scene.Meshes[i].Name + (i+1));
                child.transform.parent = parent.transform;
                var filter = child.AddComponent<MeshFilter>();
                var renderer = child.AddComponent<MeshRenderer>();
                var material = child.AddComponent<Material>();                

                material.shader = shader;

                material.uvScale = new Vector2(1.0f, 1.0f);
                material.skyColor = new Color32(255, 255, 255, 255);
                material.diffuseColor = new Color32(255, 255, 255, 255);

                int matIndex = scene.Meshes[i].MaterialIndex;

                var textures = scene.Materials[matIndex].GetAllMaterialTextures();
                var mat = scene.Materials[matIndex];

                if (mat.HasBlendMode)
                    material.alphaBlend = true;

                if(textures != null)
                {
                    if(textures.Length > 0)
                    {

                        var texturePathComponents = textures[0].FilePath.Split('\\');
                        string texturePath = texturePathComponents[texturePathComponents.Length - 1];
                        texturePath = directory + "/textures/" + texturePath;

                        //Debug.Log(texturePath);

                        if(System.IO.File.Exists(texturePath))
                            material.textures.Add(Resources.Load<Texture>(texturePath));
                        else
                            material.textures.Add(texture);
                        
                    }
                    else
                    {
                        material.textures.Add(texture);
                    }
                }


                Mesh mesh = FromAssimp(scene.Meshes[i]);

                filter.mesh = mesh;
            }

            return parent;
        }



        private static Mesh FromAssimp(Assimp.Mesh mesh)
        {
            Mesh newMesh = new Mesh();
            newMesh.indices = mesh.GetUnsignedIndices();

            bool hasTexCoords = mesh.HasTextureCoords(0);
            List<Assimp.Vector3D> uvs = hasTexCoords ? mesh.TextureCoordinateChannels[0] : null;

            // bounding box values
            float min_x, max_x, min_y, max_y, min_z, max_z;
            min_x = max_x = mesh.Vertices[0].X;
            min_y = max_y = mesh.Vertices[0].Y;
            min_z = max_z = mesh.Vertices[0].Z;

            Vertex[] vertices = new Vertex[mesh.VertexCount];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vec = new Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
                Vector3 norm = new Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);

                if (hasTexCoords)
                {
                    Assimp.Vector3D uv = uvs[i];

                    Vertex vertex = new Vertex();
                    vertex.position = vec;
                    vertex.normal = norm;
                    vertex.uv = new Vector2(uv.X, 1.0f - uv.Y);

                    vertices[i] = vertex;

                }
                else
                {
                    Vertex vertex = new Vertex();
                    vertex.position = vec;
                    vertex.normal = norm;
                    vertex.uv = Vector2.zero;
                    vertices[i] = vertex;
                }

                if (vec.x < min_x) min_x = vec.x;
                if (vec.x > max_x) max_x = vec.x;
                if (vec.y < min_y) min_y = vec.y;
                if (vec.y > max_y) max_y = vec.y;
                if (vec.z < min_z) min_z = vec.z;
                if (vec.z > max_z) max_z = vec.z;

            }

            newMesh.bounds = new BoundingBox(new Vector3(min_x, min_y, min_z), new Vector3(max_x, max_y, max_z));
            newMesh.vertices = vertices;
            return newMesh;
        }

        public static GameObject LoadFromFileTest(string filepath)
        {
            AssimpContext importer = new AssimpContext();
            var scene = importer.ImportFile(filepath, PostProcessPreset.TargetRealTimeMaximumQuality);

            if (scene.HasMaterials)
            {
                for (int ii = 0; ii < scene.MaterialCount; ++ii)
                {
                    MaterialInfo mater = ProcessMaterial(scene.Materials[ii]);
                    m_materials.Add(ii, mater);
                }
            }

            if (scene.HasMeshes)
            {
                for (int ii = 0; ii < scene.MeshCount; ++ii)
                {
                    m_meshes.Add(ProcessMesh(scene.Meshes[ii]));
                }
            }

            if (scene.RootNode != null)
            {
                Node rootNode = new Node(null);
                ProcessNode(scene, scene.RootNode, null, rootNode);
            }

            for (int i = 0; i < m_meshes.Count; i++)
            {
                Debug.Log(m_meshes[i].name);
            }

            return null;
        }

        private static MMesh ProcessMesh(Assimp.Mesh mesh)
        {
            MMesh newMesh = new MMesh();
            newMesh.name = mesh.Name.Length != 0 ? mesh.Name : "";
            newMesh.indexOffset = m_indices.Count;
            int indexCountBefore = m_indices.Count;
            int vertindexoffset = m_vertices.Count / 3;

            // Get Vertices
            if (mesh.Vertices.Count > 0)
            {
                for (int ii = 0; ii < mesh.Vertices.Count; ++ii)
                {
                    var vec = mesh.Vertices[ii];

                    m_vertices.Add(new Vector3(vec.X, vec.Y, vec.Z));
                }
            }

            // Get Normals
            if (mesh.HasNormals)
            {
                for (int ii = 0; ii < mesh.Vertices.Count; ++ii)
                {
                    var vec = mesh.Normals[ii];
                    m_normals.Add(new Vector3(vec.X, vec.Y, vec.Z));
                }
            }

            bool hasTexCoords = mesh.HasTextureCoords(0);

            if (hasTexCoords)
            {
                List<Assimp.Vector3D> uvs = hasTexCoords ? mesh.TextureCoordinateChannels[0] : null;

                if (uvs != null)
                {
                    for (int ii = 0; ii < mesh.TextureCoordinateChannels[0].Count; ++ii)
                    {
                        var vec = uvs[ii];
                        m_uvs.Add(new Vector2(vec.X, vec.Y));
                    }
                }
            }

            // Get mesh indexes
            for (int t = 0; t < mesh.Faces.Count; ++t)
            {
                var face = mesh.Faces[t];
                if (face.IndexCount != 3)
                {
                    Debug.Log("Warning: Mesh face with not exactly 3 indices, ignoring this primitive");
                    continue;
                }

                m_indices.Add(face.Indices[0] + vertindexoffset);
                m_indices.Add(face.Indices[1] + vertindexoffset);
                m_indices.Add(face.Indices[2] + vertindexoffset);
            }

            newMesh.indexCount = m_indices.Count - indexCountBefore;
            newMesh.material = m_materials[mesh.MaterialIndex];

            return newMesh;
        }


        private static MaterialInfo ProcessMaterial(Assimp.Material material)
        {
            MaterialInfo mater = new MaterialInfo();

            mater.name = material.Name;

            ShadingMode shadingModel = material.ShadingMode;

            if (shadingModel != ShadingMode.Phong && shadingModel != ShadingMode.Gouraud)
            {
                Debug.Log("This mesh's shading model is not implemented in this loader, setting to default material");
                mater.name = "DefaultMaterial";
            }
            else
            {
                Color4D dif = new Color4D(0.0f, 0.0f, 0.0f, 0.0f);
                Color4D amb = new Color4D(0.0f, 0.0f, 0.0f, 0.0f);
                Color4D spec = new Color4D(0.0f, 0.0f, 0.0f, 0.0f);
                float shine = 0.0f;

                amb = material.ColorAmbient;
                dif = material.ColorDiffuse;
                spec = material.ColorSpecular;
                shine = material.Shininess;

                mater.ambient = new Vector3(amb.R, amb.G, amb.B);
                mater.diffuse = new Vector3(dif.R, dif.G, dif.B);
                mater.specular = new Vector3(spec.R, spec.G, spec.B);
                mater.shininess = shine;

                mater.ambient *= 0.2f;

                if (mater.shininess.NearlyEquals(0.0f, float.Epsilon))
                    mater.shininess = 30;
            }

            return mater;
        }

        private static void ProcessNode(Scene scene, Assimp.Node node, Node parentNode, Node newNode)
        {

            newNode.name = node.Name.Length != 0 ? node.Name : "";

            newNode.transformation = new Matrix4x4(node.Transform);
            newNode.meshes = new List<MMesh>(new MMesh[node.MeshCount]);

            for (int imesh = 0; imesh < node.MeshCount; ++imesh)
            {
                MMesh mesh = m_meshes[node.MeshIndices[imesh]];
                newNode.meshes[imesh] = mesh;
            }

            for (int ich = 0; ich < node.ChildCount; ++ich)
            {
                newNode.nodes.Add(new Node(newNode));
                ProcessNode(scene, node.Children[ich], parentNode, newNode.nodes[ich]);
            }
        }

    }
}
