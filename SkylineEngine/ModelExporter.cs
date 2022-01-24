namespace SkylineEngine
{
    public enum AssimpFileType
    {
        FBX,
        OBJ,
        DAE
    }

    public static class ModelExporter
    {
        public static string GetExtension(AssimpFileType type)
        {
            string extension = string.Empty;

            switch(type)
            {
                case AssimpFileType.DAE:
                    extension = ".dae";
                    break;
                case AssimpFileType.FBX:
                    extension = ".fbx";
                    break;
                case AssimpFileType.OBJ:
                    extension = ".obj";
                    break;

            }
            return extension;
        }

        public static bool Export(AssimpFileType type, Mesh mesh, string filepath)
        {
            filepath = filepath + GetExtension(type);

            Assimp.Scene scene = new Assimp.Scene();
            scene.RootNode = new Assimp.Node("Root");

            Assimp.Mesh vertices = new Assimp.Mesh("", Assimp.PrimitiveType.Triangle);
            vertices.MaterialIndex = 0;

            for(int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices.Vertices.Add(mesh.vertices[i].position.ToAssimpVector3D());               
                vertices.Normals.Add(mesh.vertices[i].normal.ToAssimpVector3D());
            }

            for(int i = 0; i < mesh.indices.Length; i+=3)
            {
                Assimp.Face face = new Assimp.Face();
                face.Indices.Add((int)mesh.indices[i+0]);
                face.Indices.Add((int)mesh.indices[i+1]);
                face.Indices.Add((int)mesh.indices[i+2]);
                vertices.Faces.Add(face);
            }

            scene.Meshes.Add(vertices);
            scene.RootNode.MeshIndices.Add(0);

            Assimp.Material mat = new Assimp.Material();
            mat.Name = "Material";
            scene.Materials.Add(mat);

            Assimp.AssimpContext context = new Assimp.AssimpContext();
            return context.ExportFile(scene, filepath, "collada");
        }

        public static bool ExportObj(Mesh mesh, string filepath)
        {
            string vertices = "";
            string normals = "";
            string uvs = "";
            string indices = "";

            for(int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices += "v " + mesh.vertices[i].position + "\n";
                normals += "vn " + mesh.vertices[i].normal + "\n";
                uvs += "vt " + mesh.vertices[i].uv + "\n";
            }

            int triangleCount = mesh.indices.Length / 3;

            for(uint i = 0; i < triangleCount; i++)
            {
                uint tIndex = i * 3;
                uint vi1 = mesh.indices[tIndex];
                uint vi2 = mesh.indices[tIndex + 1];
                uint vi3 = mesh.indices[tIndex + 2];
                indices += "f " + vi1 + " " + vi2 + " " + vi3 + "\n";
            }

            vertices += "\n";
            normals += "\n";
            uvs += "\n";

            string outputText = vertices + uvs + normals + indices;

            System.IO.File.WriteAllText(filepath, outputText);

            return true;
        }    

        public static Assimp.Vector3D ToAssimpVector3D(this Vector3 v) 
        {
            return new Assimp.Vector3D(v.x, v.y, v.z);
        }
    }
}
