using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

public class NewBehaviourScript1 : MonoBehaviour
{

    readonly int ChunkSize = 145;
    public static float MaxViewDistance = 200f;
    Dictionary<Vector2, TerrainChunk> TerrainDict;
    readonly List<TerrainChunk> TerrainList = new();

    int VisibleChunksDistance;
    public Transform Viewer;
    public static Vector2 ViewerPos;

    private void Start()
    {

        VisibleChunksDistance = Mathf.RoundToInt(MaxViewDistance / ChunkSize);
        TerrainDict = new Dictionary<Vector2, TerrainChunk>();
    }
    private void Update()
    {

        ViewerPos = new Vector2(Viewer.position.x, Viewer.position.z);
        CalcVisibleChunks();

    }
    public void CalcVisibleChunks()
    {
        int currentViewerPosX = Mathf.RoundToInt(ViewerPos.x / ChunkSize);
        int currentViewerPosY = Mathf.RoundToInt(ViewerPos.y / ChunkSize);

        for (int i = 0; i < TerrainList.Count; i++)
        {
            if (Vector3.Distance(TerrainList[i].GetTerrainPos(), ViewerPos) > MaxViewDistance) { TerrainList[i].SetOff(); }
        }
        TerrainList.Clear();

        for (int x = -VisibleChunksDistance; x <= VisibleChunksDistance; x++)
        {
            for (int y = -VisibleChunksDistance; y <= VisibleChunksDistance; y++)
            {
                Vector2 visibleChunkCoord = new(currentViewerPosX + x, currentViewerPosY + y);

                if (TerrainDict.ContainsKey(visibleChunkCoord))
                {
                    TerrainDict[visibleChunkCoord].UpdateTerrain();
                    TerrainList.Add(TerrainDict[visibleChunkCoord]);
                }
                else
                {
                    TerrainDict.Add(visibleChunkCoord, new TerrainChunk(visibleChunkCoord, ChunkSize));
                }
            }
        }
    }


    public struct TerrainChunk
    {//+chunksize/2?!
        Vector3 m_Pos;
        readonly GameObject m_terrainObject;
        Bounds edge;

        public TerrainChunk(Vector2 pos, int size)
        {
            m_Pos = new Vector3(pos.x * size, 0, pos.y * size);
            edge = new Bounds(m_Pos, Vector2.one * size);
            m_terrainObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            m_terrainObject.transform.position = m_Pos;
            m_terrainObject.transform.localScale = Vector3.one * size / 10;
        }
        public void UpdateTerrain()
        {
            float playerDistanceFromEdge = Mathf.Sqrt(edge.SqrDistance(new Vector3(ViewerPos.x, 0, ViewerPos.y)));
            if (playerDistanceFromEdge >= MaxViewDistance)
                SetOff();
            else
                SetOn();
        }
        public readonly void TerrainActive(bool x) { m_terrainObject.SetActive(x); }
        public readonly void SetOff() { m_terrainObject.SetActive(false); }
        public readonly void SetOn() { m_terrainObject.SetActive(true); }
        public readonly Vector3 GetTerrainPos() { return m_Pos; }
    }
}