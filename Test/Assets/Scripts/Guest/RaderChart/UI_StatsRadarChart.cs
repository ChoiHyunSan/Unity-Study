using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatsRadarChart : MonoBehaviour {

    [SerializeField] private Material radarMaterial;
    [SerializeField] private Texture2D radarTexture2D;

    private Stats stats;
    private CanvasRenderer radarMeshCanvasRenderer;

    private void Awake() {
        radarMeshCanvasRenderer = transform.Find("radarMesh").GetComponent<CanvasRenderer>();
    }

    public void SetStats(Stats stats) {
        this.stats = stats;
        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsVisual();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e) {
        UpdateStatsVisual();
    }

    private void UpdateStatsVisual() {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[6];
        Vector2[] uv = new Vector2[6];
        int[] triangles = new int[3 * 5];

        float angleIncrement = 360f / 5;
        float radarChartSize = 145f;

        Vector3 JoyVertex = Quaternion.Euler(0, 0, -angleIncrement * 0) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Joy);
        int JoyVertexIndex = 1;
        Vector3 SadVertex = Quaternion.Euler(0, 0, -angleIncrement * 1) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Sad);
        int SadVertexIndex = 2;
        Vector3 FearVertex = Quaternion.Euler(0, 0, -angleIncrement * 2) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Fear);
        int FearVertexIndex = 3;
        Vector3 CalmVertex = Quaternion.Euler(0, 0, -angleIncrement * 3) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Calm);
        int CalmVertexIndex = 4;
        Vector3 AngerIndex = Quaternion.Euler(0, 0, -angleIncrement * 4) * Vector3.up * radarChartSize * stats.GetStatAmountNormalized(Stats.Type.Anger);
        int AngerVertexIndex = 5;

        vertices[0] = Vector3.zero;
        vertices[JoyVertexIndex]  = JoyVertex;
        vertices[SadVertexIndex] = SadVertex;
        vertices[FearVertexIndex]   = FearVertex;
        vertices[CalmVertexIndex]    = CalmVertex;
        vertices[AngerVertexIndex]  = AngerIndex;

        uv[0]                   = Vector2.zero;
        uv[JoyVertexIndex]   = Vector2.one;
        uv[SadVertexIndex]  = Vector2.one;
        uv[FearVertexIndex]    = Vector2.one;
        uv[CalmVertexIndex]     = Vector2.one;
        uv[AngerVertexIndex]   = Vector2.one;

        triangles[0] = 0;
        triangles[1] = JoyVertexIndex;
        triangles[2] = SadVertexIndex;

        triangles[3] = 0;
        triangles[4] = SadVertexIndex;
        triangles[5] = FearVertexIndex;

        triangles[6] = 0;
        triangles[7] = FearVertexIndex;
        triangles[8] = CalmVertexIndex;

        triangles[9]  = 0;
        triangles[10] = CalmVertexIndex;
        triangles[11] = AngerVertexIndex;

        triangles[12] = 0;
        triangles[13] = AngerVertexIndex;
        triangles[14] = JoyVertexIndex;


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        radarMeshCanvasRenderer.SetMesh(mesh);
        radarMeshCanvasRenderer.SetMaterial(radarMaterial, radarTexture2D);
    }

}
