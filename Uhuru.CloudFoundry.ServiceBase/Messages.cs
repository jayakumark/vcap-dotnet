﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uhuru.Utilities;
using System.Globalization;

namespace Uhuru.CloudFoundry.ServiceBase
{
    interface IWithSuccessStatus
    {
        bool Success
        {
            get;
            set;
        }

        Dictionary<string, object> Error
        {
            get;
            set;
        }

        string ToJson();
    }

    class ProvisionRequest
    {
        public ProvisionedServicePlanType Plan
        {
            get;
            set;
        }

        public ServiceCredentials Credentials
        {
            get;
            set;
        }

        public void FromJson(string json)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject = jsonObject.FromJson(json);
            Plan = (ProvisionedServicePlanType)Enum.Parse(typeof(ProvisionedServicePlanType), jsonObject["plan"].ToValue<string>(), true);
            if (jsonObject.ContainsKey("credentials"))
            {
                Credentials = new ServiceCredentials();
                Credentials.FromJson(jsonObject["credentials"].ToJson());
            }
        }
    }

    // Node --> Provisioner
    class ProvisionResponse : IWithSuccessStatus
    {
        public bool Success
        {
            get;
            set;
        }
        public ServiceCredentials Credentials
        {
            get;
            set;
        }
        public Dictionary<string, object> Error
        {
            get;
            set;
        }

        public string ToJson()
        {
            return new Dictionary<string, object>()
              {
                  {"success", Success},
                  {"credentials", Credentials.ToDictionary()},
                  {"error", Error}
              }.ToJson();
        }
    }


    class UnprovisionRequest
    {
        public string Name
        {
            get;
            set;
        }
        public ServiceCredentials[] Bindings
        {
            get;
            set;
        }

        public void FromJson(string json)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject = jsonObject.FromJson(json);

            Name = jsonObject["name"].ToValue<string>();

            object[] objBindings = jsonObject["bindings"].ToObject<object[]>();
            Bindings = new ServiceCredentials[objBindings.Length];

            for (int i = 0; i < objBindings.Length; i++)
            {
                Bindings[i] = new ServiceCredentials();
                Bindings[i].FromJson(objBindings[i].ToJson());
            }
        }
    }

    class BindRequest
    {
        public string Name
        {
            get;
            set;
        }
        public Dictionary<string, object> BindOptions
        {
            get;
            set;
        }
        public ServiceCredentials Credentials
        {
            get;
            set;
        }

        public void FromJson(string json)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject = jsonObject.FromJson(json);

            Name = jsonObject["name"].ToValue<string>();

            if (jsonObject.ContainsKey("bind_opts"))
            {
                BindOptions = new Dictionary<string, object>();
                BindOptions = jsonObject["bind_opts"].ToObject<Dictionary<string, object>>();
            }

            if (jsonObject.ContainsKey("credentials"))
            {
                Credentials = new ServiceCredentials();
                Credentials.FromJson(jsonObject["credentials"].ToJson());
            }
        }
    }

    class BindResponse : IWithSuccessStatus
    {
        public bool Success
        {
            get;
            set;
        }
        public ServiceCredentials Credentials
        {
            get;
            set;
        }
        public Dictionary<string, object> Error
        {
            get;
            set;
        }

        public string ToJson()
        {
            return new Dictionary<string, object>()
              {
                  {"success", Success},
                  {"credentials", Credentials.ToDictionary()},
                  {"error", Error}
              }.ToJson();
        }
    }

    class UnbindRequest
    {
        public ServiceCredentials Credentials
        {
            get;
            set;
        }

        public void FromJson(string json)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject = jsonObject.FromJson(json);
            Credentials = new ServiceCredentials();
            Credentials.FromJson(jsonObject["credentials"].ToJson());
        }
    }

    class SimpleResponse : IWithSuccessStatus
    {
        public bool Success
        {
            get;
            set;
        }
        public Dictionary<string, object> Error
        {
            get;
            set;
        }

        public string ToJson()
        {
            return new Dictionary<string, object>()
              {
                  {"success", Success},
                  {"error", Error}
              }.ToJson();
        }
    }

    class RestoreRequest
    {
        public string InstanceId
        {
            get;
            set;
        }
        public string BackupPath
        {
            get;
            set;
        }

        public void FromJson(string json)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject = jsonObject.FromJson(json);

            InstanceId = jsonObject["instance_id"].ToValue<string>();
            BackupPath = jsonObject["backup_path"].ToValue<string>();
        }
    }

    class CheckOrphanRequest
    {
        public Handle[] Handles
        {
            get;
            set;
        }

        public void FromJson(string json)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject = jsonObject.FromJson(json);

            object[] handlesArray = jsonObject["handles"].ToObject<object[]>();

            Handles = new Handle[handlesArray.Length];

            for (int i = 0; i < handlesArray.Length; i++)
            {
                Handles[i] = new Handle();
                Handles[i].FromJson(handlesArray[i].ToJson());
            }
        }
    }

    class CheckOrphanResponse : IWithSuccessStatus
    {
        public bool Success
        {
            get;
            set;
        }
        public Dictionary<string, object> Error
        {
            get;
            set;
        }
        // A hash for orphan instances
        // Key: the id of the node with orphans
        // Value: orphan instances list
        public Dictionary<string, object> OrphanInstances
        {
            get;
            set;
        }
        // A hash for orphan bindings
        // Key: the id of the node with orphans
        // Value: orphan bindings list
        public Dictionary<string, object> OrphanBindings
        {
            get;
            set;
        }

        public string ToJson()
        {
            return new Dictionary<string, object>()
              {
                  {"success", Success},
                  {"error", Error},
                  {"orphan_instances", OrphanInstances},
                  {"orphan_instances", OrphanBindings}
              }.ToJson();
        }
    }

    class PurgeOrphanRequest
    {
        // A list of orphan instances names
        public string[] OrphanInsList
        {
            get;
            set;
        }
        // A list of orphan bindings credentials
        public ServiceCredentials[] OrphanBindingList
        {
            get;
            set;
        }

        public void FromJson(string json)
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            jsonObject = jsonObject.FromJson(json);

            OrphanInsList = jsonObject["orphan_ins_list"].ToObject<string[]>();

            object[] objBindingList = jsonObject["orphan_binding_list"].ToObject<object[]>();
            OrphanBindingList = new ServiceCredentials[objBindingList.Length];

            for (int i = 0; i < objBindingList.Length; i++)
            {
                OrphanBindingList[i] = new ServiceCredentials();
                OrphanBindingList[i].FromJson(objBindingList[i].ToJson());
            }

        }
    }
}