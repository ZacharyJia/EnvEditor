using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace EnvEditor
{
    class EnvService
    {

        public List<EnvVar> GetEnvs()
        {
            var variables = (Hashtable)Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User);
            List<EnvVar> list = new List<EnvVar>();
            foreach(DictionaryEntry item in variables)
            {
                list.Add(new EnvVar((string)item.Key, (string)item.Value));
            }
            return list;
        }

        public void SetEnvs(List<EnvVar> list)
        {
            foreach(var item in list)
            {
                Environment.SetEnvironmentVariable(item.Key, item.Value, EnvironmentVariableTarget.User);
            }
        }
    }
}
