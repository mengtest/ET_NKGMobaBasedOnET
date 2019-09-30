//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:44:40
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using ETModel.TheDataContainsAction;
using MongoDB.Bson.Serialization;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class NP_RuntimeTreeRepositoryAwakeSystem: AwakeSystem<NP_TreeDataRepository>
    {
        public override void Awake(NP_TreeDataRepository self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 行为树数据仓库组件
    /// </summary>
    public class NP_TreeDataRepository: Component
    {
        public const string NPDataPath = "../Config/NPBehaveConfig/";

        public Dictionary<long, NP_DataSupportor> NpRuntimeTreesDatas = new Dictionary<long, NP_DataSupportor>();

        public void Awake()
        {
            Type[] types = typeof (NodeType).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (NP_NodeDataBase)) && !type.IsSubclassOf(typeof (NP_ClassForStoreAction)) &&
                    !type.IsSubclassOf(typeof (SkillBaseNodeData)) && !type.IsSubclassOf(typeof (BuffDataBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            DirectoryInfo directory = new DirectoryInfo(NPDataPath);
            FileInfo[] fileInfos = directory.GetFiles();

            foreach (var VARIABLE in fileInfos)
            {
                byte[] mfile = File.ReadAllBytes(VARIABLE.FullName);

                if (mfile.Length == 0) Log.Info("没有读取到文件");

                try
                {
                    NP_DataSupportor MnNpDataSupportor = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);
                
                    Log.Info("反序列化行为树完成");
                    
                    NpRuntimeTreesDatas.Add(MnNpDataSupportor.RootId, MnNpDataSupportor);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }
        }

        /// <summary>
        /// 获取一棵树的所有数据（通过深拷贝形式）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NP_DataSupportor GetNP_TreeData_DeepCopy(long id)
        {
            if (this.NpRuntimeTreesDatas.ContainsKey(id))
            {
                return NpRuntimeTreesDatas[id].DeepCopy();
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }
    }
}