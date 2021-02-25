
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;

public class BaseLoadProcess
{
    //loader 缓存池
    //public List<BaseLoader> loaderCache;

    //准备中的 loader 列表
    public List<BaseLoader> preparingList;
    //等待加载队列
    public List<BaseLoader> waitLoadQueue;
    //正在加载中队列
    public List<BaseLoader> loadingQueue;

    private Action<BaseLoader> loadFinishCallback;

    public int maxLoadingCount = 5;

    public void Init()
    {

    }

    internal void SetFinishCallback(Action<BaseLoader> loadFinishCallback)
    {
        this.loadFinishCallback = loadFinishCallback;
    }
    
    public void Update(float timeDelta)
    {
        //正在加载队列
        var finishLoadList = new List<BaseLoader>();
        for (int i = 0; i < loadingQueue.Count; i++)
        {
            var loadingLoader = loadingQueue[i];

            loadingLoader.Update(timeDelta);

            if (loadingLoader.IsLoadFinish())
            {
                finishLoadList.Add(loadingLoader);
            }
        }

        //移除已经加载完成的 loader
        //finishList

        //从准备中列表 中 挑出准备完毕的 loader 进入 等待加载队列
        var finishPrepareList = new List<BaseLoader>();
        for (int i = 0; i < preparingList.Count; i++)
        {
            var prepareLoader = preparingList[i];
            
            if (prepareLoader.IsPrepareFinish())
            {
                finishPrepareList.Add(prepareLoader);
            }
        }

        //移除已经准备完成的 loader
        //finishPrepareList


        //检查正在加载队列有没有空闲位置 有的话从 等待加载队列 中取 loader 填满正在加载队列
        
    }

    public virtual void AddLoader(BaseLoader loader)
    {
        preparingList.Add(loader);
        loader.Start();
        //loaderCache.Add(loader);
    }

    public virtual void OnPrepareFinish(BaseLoader loader)
    {
        loader.OnPrepareFinish();
    }

    public virtual void OnLoadFinish(BaseLoader loader)
    {
        this.loadFinishCallback?.Invoke(loader);

        //loader.OnLoadFinish();
    }


}
