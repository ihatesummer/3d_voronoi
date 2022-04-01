using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ImportData : MonoBehaviour
{
    public GameObject silicaParent;
    public GameObject vorParent;
    public string silicaPosFile;
    public string vorVertexPosFile;
    public string vorDictFile;
    public int silicaCount = 0;
    public int vorVertexCount = 0;
    public List<Vector3> listXYZ_Silica = new List<Vector3>();
    public List<Vector3> listXYZ_vorVertex = new List<Vector3>();
    public Material m_vorVertex;
    public Dictionary<(int, int), List<int>> vorDict;

    // Start is called before the first frame update
    void Start()
    {
        silicaParent = GameObject.Find("Silica");
        vorParent = GameObject.Find("Voronoi");
        silicaPosFile = Application.dataPath + "/Data/points.csv";
        vorVertexPosFile = Application.dataPath + "/Data/voronoi_vertices.csv";
        vorDictFile = Application.dataPath + "/Data/voronoi_dict.json";

        int count;

        listXYZ_Silica = parsePositions(silicaPosFile, out count);
        silicaCount = count;

        listXYZ_vorVertex = parsePositions(vorVertexPosFile, out count);
        vorVertexCount = count;

        int idx = 0;
        foreach (Vector3 v in listXYZ_Silica)
        {
            idx++;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = v;
            sphere.name = "Silica" + idx.ToString();
            sphere.transform.SetParent(silicaParent.transform);
        }

        idx = 0;
        foreach (Vector3 v in listXYZ_vorVertex)
        {
            idx++;
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = v;
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            sphere.name = "Vertex" + idx.ToString();
            sphere.transform.SetParent(vorParent.transform);
            sphere.GetComponent<MeshRenderer>().material = m_vorVertex;
        }
        

        using (StreamReader sr = new StreamReader(vorDictFile))
        {
            string json_text = sr.ReadLine();
            count = 0;
            Debug.Log(json_text);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Vector3> parsePositions(string filename, out int count) {
        using (StreamReader sr = new StreamReader(filename))
        {
            string line;
            count = 0;
            List<Vector3> listXYZ = new List<Vector3>();
            while ((line = sr.ReadLine()) != null)
            {
                count++;
                string[] coords = line.Split(',');
                Vector3 v = new Vector3(float.Parse(coords[0]),
                                        float.Parse(coords[1]),
                                        float.Parse(coords[2]));
                listXYZ.Add(v);
            }
            return listXYZ;
        }
    }

}
