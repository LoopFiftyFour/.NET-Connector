using System;

namespace Loop54.Model.CollectionItems
{
    public class Item<T>
    {
        public virtual T Key { get; set; }
        public double Value;

        public override string ToString()
        {
            return "{" + Key + ":" + Value + "}";
        }

        internal Item()
        {
            
        } 
    }

    public class V22EntityItem: Item<Entity>
    {
        public Entity Entity;

        public override Entity Key
        {
            get { return Entity; }
        }

        internal V22EntityItem()
        {
            
        }
    }

    public class V22StringItem : Item<String>
    {
        public String String;

        public override String Key
        {
            get { return String; }
        }

        internal V22StringItem()
        {

        }
    }
}
