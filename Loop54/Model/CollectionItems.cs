using System;

namespace Loop54.Model.CollectionItems
{
    public class Item<T>
    {
        public virtual T Key { get; private set; }
        public double Value { get; private set; }

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
        public Entity Entity { get; private set; }

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
        public String String { get; private set; }

        public override String Key
        {
            get { return String; }
        }

        internal V22StringItem()
        {

        }
    }
}
