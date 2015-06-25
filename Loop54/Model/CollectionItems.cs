using System;

namespace Loop54.Model.CollectionItems
{
    /// <summary>
    /// Type for collections returned from Engine.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Item<T>
    {
        public virtual T Key { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return "{" + Key + ":" + Value + "}";
        }

        internal Item()
        {
            
        } 
    }

    /// <summary>
    /// Compatibility type to be used when deserializing responses from V2.2 or older engines.
    /// </summary>
    public class V22EntityItem: Item<Entity>
    {
        public Entity Entity { get; set; }

        public override Entity Key
        {
            get { return Entity; }
        }

        internal V22EntityItem()
        {
            
        }
    }

    /// <summary>
    /// Compatibility type to be used when deserializing responses from V2.2 or older engines.
    /// </summary>
    public class V22StringItem : Item<String>
    {
        public String String { get; set; }

        public override String Key
        {
            get { return String; }
        }

        internal V22StringItem()
        {

        }
    }
}
