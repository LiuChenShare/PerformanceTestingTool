using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PerformanceTestingTool
{
    /// <summary>
    /// 性能试验
    /// </summary>
    public class PerformanceTest
    {
        private DateTime BeginTime;
        private DateTime EndTime;
        private ParamsModel Params;

        /// <summary>
        ///设置执行次数(默认:1)
        /// </summary>
        public void SetCount(int count)
        {
            Params.RunCount = count;
        }
        /// <summary>
        /// 设置线程模式(默认:false)
        /// </summary>
        /// <param name="isMul">true为多线程</param>
        public void SetIsMultithread(bool isMul)
        {
            Params.IsMultithread = isMul;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceTest()
        {
            Params = new ParamsModel()
            {
                RunCount = 1
            };
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="action">待测试方法</param>
        /// <param name="parameter">参数列表-若无对应执行序号的参数，则传递上一执行序号的参数</param>
        /// <param name="rollBack"></param>
        public void Execute(Action<string> action, string[] parameters, Action<string> rollBack)
        {
            List<Thread> arr = new List<Thread>();
            if(parameters == null)
            {
                parameters = new string[] {null };
            }
            BeginTime = DateTime.Now;
            for (int i = 0; i < Params.RunCount; i++)
            {
                string parameter = null;
                if (parameters != null && parameters.Count() > i)
                {
                    parameter = parameters[i];
                }
                else
                {
                    parameter = parameters.LastOrDefault();
                }
                if (Params.IsMultithread)
                {
                    var thread = new Thread(new System.Threading.ThreadStart(() =>
                    {
                        action(parameter);
                    }));
                    thread.Start();
                    arr.Add(thread);
                }
                else
                {
                    action(parameter);
                }
            }
            if (Params.IsMultithread)
            {
                foreach (Thread t in arr)
                {
                    while (t.IsAlive)
                    {
                        Thread.Sleep(10);
                    }
                }

            }
            rollBack(GetResult());
        }

        public string GetResult()
        {
            EndTime = DateTime.Now;
            var time = (EndTime - BeginTime).TotalMilliseconds;
            var average = (time / Params.RunCount).ToString("n5");
            string totalTime = time.ToString("n5");
            string reval = string.Format("总共执行时间：{0}毫秒 ,平均执行时间：{1}毫秒 。", totalTime, average);
            if (Params.IsMultithread)
            {
                reval = string.Format("总共执行时间：{0}毫秒 。", totalTime);
            }
            Console.Write(reval);
            return reval;
        }

        private class ParamsModel
        {
            public int RunCount { get; set; }
            /// <summary>
            /// 是否为多线程
            /// </summary>
            public bool IsMultithread { get; set; }
        }
    }
}
