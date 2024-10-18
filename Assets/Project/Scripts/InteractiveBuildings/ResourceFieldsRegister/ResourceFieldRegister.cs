using System;
using System.Linq;
using Project.Scripts.Arch.Building;
using UnityEngine;

namespace Project.Scripts.InteractiveBuildings.ResourceFieldsRegister
{
    public class ResourceFieldRegister
    {
        private InteractionField[] fields;

        public ResourceFieldRegister()
        { 
            //находим максимальное значение enum`а
            ResourceType maxResource = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().Max();
            //создаем массив на все возможные виды ресурсов
            fields = new InteractionField[(int)maxResource+1];
        }

        public void RegisterResourceRecieveField(InteractionField field, ResourceType type)
        {
            fields[(int)type] = field;
        }

        public InteractionField GetFieldOfResourceType(ResourceType type)
        {
            return fields[(int)type];
        }
    }
}