using System.Collections.Generic;
using Assimp;
using SkylineEngine.Utilities;

namespace SkylineEngine
{
    public class ModelImporter2
    {
        public struct MaterialInfo
        {
            public string name;
            public Color32 ambient;
            public Color32 diffuse;
            public Color32 specular;
            public float shininess;
            public List<Texture> textures;
        }

        public class aiMesh
        {
            public string name;
            public int indexOffset;
            public int indexCount;
            public MaterialInfo material;
            public List<Vertex> vertices;
            public List<uint> indices;
        }

        private List<aiMesh> meshes = new List<aiMesh>();
        
        public GameObject Load(string filepath)
        {
            meshes.Clear();

            AssimpContext importer = new AssimpContext();
            var scene = importer.ImportFile(filepath, PostProcessPreset.TargetRealTimeMaximumQuality);

            ProcessNode(scene.RootNode, scene);

            if(meshes.Count > 0)
            {
                GameObject obj = new GameObject();

                for(int i = 0; i < meshes.Count; i++)
                {
                    GameObject child = new GameObject();
                    child.transform.parent = obj.transform;

                    var meshRenderer = child.AddComponent<MeshRenderer>();
                    var meshFilter = child.AddComponent<MeshFilter>();
                    var material = child.AddComponent<Material>();
                    material.shader = Resources.Load<Shader>("res/Shaders/Default.shader");

                    var mesh = new Mesh();
                    mesh.vertices = meshes[i].vertices.ToArray();
                    mesh.indices = meshes[i].indices.ToArray();

                    for(int j = 0; j < meshes[i].material.textures.Count; j++)
                    {
                        material.textures.Add(meshes[i].material.textures[j]);
                    }

                    meshFilter.mesh = mesh;

                }

                return obj;
            }

            return null;
        }

        private void ProcessNode(Assimp.Node node, Assimp.Scene scene)
        {
            for(int i = 0; i < node.MeshCount; i++)
            {
                var mesh = scene.Meshes[node.MeshIndices[i]];
                meshes.Add(ProcessMesh(mesh, scene));
            }

            for(int i = 0; i < node.ChildCount; i++)
            {
                ProcessNode(node.Children[i], scene);
            }
        }

        private aiMesh ProcessMesh(Assimp.Mesh mesh, Assimp.Scene scene)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();
            List<Texture> textures = new List<Texture>();

            for(int i = 0; i < mesh.Vertices.Count; i++)
            {
                Vertex vertex = new Vertex();
                vertex.position = new Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
                vertex.normal = new Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);

                if(mesh.TextureCoordinateChannelCount > 0)
                    vertex.uv = new Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);
                else
                    vertex.uv = new Vector2(0, 0);

                vertices.Add(vertex);
            }            

            for(int i = 0; i < mesh.FaceCount; i++)
            {
                for(int j = 0; j < mesh.Faces[i].Indices.Count; j++)
                {
                    indices.Add((uint)mesh.Faces[i].Indices[j]);
                }
            }

            var materialInfo = new MaterialInfo();

            if(mesh.MaterialIndex >= 0)
            {
                var material = scene.Materials[mesh.MaterialIndex];
                
                materialInfo.ambient = new Color32(material.ColorAmbient.R, material.ColorAmbient.G, material.ColorAmbient.B, material.ColorAmbient.A);
                materialInfo.diffuse = new Color32(material.ColorDiffuse.R, material.ColorDiffuse.G, material.ColorDiffuse.B, material.ColorDiffuse.A);
                materialInfo.specular = new Color32(material.ColorSpecular.R, material.ColorSpecular.G, material.ColorSpecular.B, material.ColorSpecular.A);
                materialInfo.shininess = material.Shininess;
                materialInfo.name = material.Name;
                materialInfo.textures = LoadTextures(material, TextureType.Diffuse);                
            }

            aiMesh result = new aiMesh();
            result.vertices = vertices;
            result.indices = indices;
            result.material = materialInfo;            

            return result;
        }

        private List<Texture> LoadTextures(Assimp.Material material, Assimp.TextureType type)
        {
            List<Texture> textures = new List<Texture>();

            for(int i = 0; i < material.GetMaterialTextureCount(type); i++)
            {
                TextureSlot slot;
                material.GetMaterialTexture(type, i, out slot);

                Debug.Log(slot.FilePath);

                Texture texture = null;

                if(System.IO.File.Exists(slot.FilePath))
                    texture = Resources.Load<Texture>(slot.FilePath);
                else
                    continue;
                
                bool exists = false;
                for(int j = 0; j < textures.Count; j++)
                {
                    if(textures[j].name == texture.name)
                    {
                        exists = true;
                        break;
                    }
                }

                if(!exists)
                    textures.Add(texture);
            }

            return textures;
        }
    }
}