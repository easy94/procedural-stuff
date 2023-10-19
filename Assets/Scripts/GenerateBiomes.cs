public static class GenerateBiomes
{

    public static OozeType GenerateRndmBiomes(int mapsize, int gridX, int amountOfBiomes)
    {
        OozeType hexagonGrid = new(mapsize,gridX);

        //init oozetypes here********************************************
        int r = 8;


        for (int i = 0; i < r; i++)
        {
            hexagonGrid.OozeProcess((i%amountOfBiomes) +1);
        }


        return hexagonGrid;
    }

}