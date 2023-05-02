using PseudoEntityFramework.Abstraction;
using PseudoEntityFramework.Models;
using System.Reflection;

namespace ETeamProjectPersistence.Contexts
{
    public class ETeamProjectDbContext : DbContext
    {

        /*
            path is parameter which says, where to create database and DatabaseName
         if Name of folder where base will be located
         */
        public ETeamProjectDbContext(string path, string DataBaseName) : 
            base(path + $"{DataBaseName}\\", GetAllEntities())
        {

        }



        /*
            This method will get all classes from this assembly which is child of 
         BaseEntity
        */
        public static Type[] GetAllEntities()
        {
            Type parentType = typeof(BaseEntity);
            Assembly assembly = Assembly.GetExecutingAssembly();
            
            Type[] types = assembly.GetTypes();//Getting all class types

            //Getting all classes which are childs of Base entity
            Type[] Entites = types.Where(t => parentType.IsAssignableFrom(t) && t != parentType).ToArray();
            return Entites;
        }
    }
}
