using PseudoEntityFramework.Models;
using System.Text.Json;
using PseudoEntityFramework.Errors;
using System.Xml;

namespace PseudoEntityFramework.Abstraction
{
    public abstract class DbContext
    {
        private string BaseDatabaseLocation;


        //informations about sets and their location
        private Dictionary<string, string> TypePath;


        public DbContext(string BasePath, params Type[] Types)
        {
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            this.BaseDatabaseLocation = BasePath;
            TypePath = new Dictionary<string, string>();
            for(int i = 0; i < Types.Length; i++)
            {
                TypePath.Add(Types[i].Name, BasePath + $"{Types[i].Name}s.txt");
            }
            foreach (KeyValuePair<string,string> file in TypePath)
            {
                if (!File.Exists(file.Value))
                {
                    File.Create(file.Value).Close();
                }
            }
        }


        /*
                              -- Usage --
                      Adds item into specified table.
                            -- Description --
        Adds item into specifed table. if there exists item with this ID,
        it throws IDAlreadyExists exception and if parameters id is empty or
        null, it throws IDNullException;
         */
        public string AddItem<T>(T item) where T : BaseEntity
        {
            if (item.ID == null || item.ID.Equals("")) throw new IDNullException();
            if (Exists<T>(item.ID)) throw new IDAlreadyExists();
            string InsertText = JsonSerializer.Serialize<T>(item);
            File.AppendAllText(TypePath[item.GetType().Name], InsertText + ",\n");
            
            return item.ID;
        }

        public void Update<T>(string ID, Action<T> action) where T : BaseEntity
        {
            List<T> Table = GetTable<T>();
            int IndexOfItem = 0;
            for(int i = 0; i < Table.Count; i++)
            {
                if (Table[i].ID.Equals(ID))
                {
                    action.Invoke(Table[i]);
                    break;
                }
            }
            File.WriteAllText(TypePath[typeof(T).Name], "");
            for(int i = 0; i < Table.Count; i++)
            {
                AddItem(Table[i]);
            }
        }

        /*
                              -- Usage --
            returns full Table as List<T> where T is type of entity
         */
        public List<T> GetTable<T>() where T : BaseEntity
        {
            string FullText = File.ReadAllText(TypePath[typeof(T).Name]);
            if (FullText.Length == 0) return new List<T>(); 
            string TextFromFile = $"[{FullText.Substring(0,FullText.Length - 2)}]";
            List<T> Table = JsonSerializer.Deserialize<List<T>>(TextFromFile);
            return Table;
        }



        /*
                               -- Usage --
            Checks if T typed entity exists in specific table. if exists
            returns true, else return false;
         */
        public bool Exists<T>(string ID) where T : BaseEntity
        {
            List<T> table = GetTable<T>();
            foreach (T item in table)
            {
                if(item.ID.Equals(ID)) return true;
            }
            return false;
        }


        /*
         Returns true if there is element which satisfies predicate otherwise
        returns false
        */
        public bool Exists<T>(Func<T,bool> predicate) where T : BaseEntity
        {
            List<T> table = GetTable<T>();
            foreach (T item in table)
            {
                if(predicate.Invoke(item)) return true;
            }
            return false;
        }


        //Returns item which satisfies predicate
        public T Get<T>(Func<T,bool> predicate) where T : BaseEntity
        {
            List<T> table = GetTable<T>();
            foreach (T item in table)
            {
                if (predicate.Invoke(item)) return item;
            }
            return null;
        }
    }
}
