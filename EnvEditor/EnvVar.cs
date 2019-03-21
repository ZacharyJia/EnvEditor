using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace EnvEditor
{
    [Serializable]
    public class EnvVar : INotifyPropertyChanged, ISerializable
    {
        private string key;
        private string value;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                if (this.key != value)
                {
                    this.key = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Key)));
                    }
                }
            }
        }
        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(nameof(Value)));
                    }
                }
            }
        }

        public EnvVar(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        protected EnvVar(SerializationInfo info, StreamingContext context)
        {
            this.key = info.GetString("key");
            this.value = info.GetString("value");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("key", this.key);
            info.AddValue("value", this.value);
        }
    }
}
