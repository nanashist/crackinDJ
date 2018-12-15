using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


//public interface IState
//{
//    IState execute(IState sts);
//}

namespace crackindj
{

    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {



            GameManager.DoInit();
            GameManager.DoXFader();

            GameManager.DoMain(testMakecdj.TestMakeCDJData());

            GameManager.DoTerminate();
        }
    }
}
