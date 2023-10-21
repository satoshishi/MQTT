namespace Networking.CoreSystem.Streaming
{
    using System;

    /// <summary>
    /// 特定のプロトコルに則ってT型の情報をやり取りするinterface
    /// </summary>
    /// <typeparam name="T">やり取りするデータを表現するクラス</typeparam>
    public interface INetworkStreamer<T>
    {
        IDisposable AddListener(Action<T> listener);
    }
}
