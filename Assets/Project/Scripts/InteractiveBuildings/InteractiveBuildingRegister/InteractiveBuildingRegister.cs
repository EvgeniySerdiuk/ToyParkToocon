using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.InteractiveBuildings;

public class InteractiveBuildingRegister
{
    public event Action<IInteractiveBuilding> OnNewBuildingRegisterEvent;
    
    private HashSet<IInteractiveBuilding>[] map;
    
    public InteractiveBuildingRegister() 
    { 
        //  создаём отдельно массив для хранения всех видов интерактивный объектов
        // (аттракицоны, фудтраки и туалеты - 3 вида)
        BuildingType maxBuildingsType = Enum.GetValues(typeof(BuildingType)).Cast<BuildingType>().Max();
        int buildingsTypeCount = (int)maxBuildingsType + 1;
        map = new HashSet<IInteractiveBuilding>[buildingsTypeCount]; 
        for (int i = 0; i < buildingsTypeCount; i++) 
            map[i] = new HashSet<IInteractiveBuilding>();
    }

    public void Register(IInteractiveBuilding building, BuildingType buildingType)
    {
        if (map[(int)buildingType].Contains(building))
            return;
        map[(int)buildingType].Add(building);
        OnNewBuildingRegisterEvent?.Invoke(building);
    }

    public HashSet<IInteractiveBuilding>[] GetBuildingsInLocation() 
    {
        return map;
    } 
}