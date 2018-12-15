using System;

namespace Plugin
{
    /// <summary>
    /// プラグインに関する情報
    /// </summary>
    public class PluginInfo
    {
        private string _location;
        private string _className;

        /// <summary>
        /// PluginInfoクラスのコンストラクタ
        /// </summary>
        /// <param name="path">アセンブリファイルのパス</param>
        /// <param name="cls">クラスの名前</param>
        private PluginInfo(string path, string cls)
        {
            this._location = path;
            this._className = cls;
        }

        /// <summary>
        /// アセンブリファイルのパス
        /// </summary>
        public string Location
        {
            get { return _location; }
        }

        /// <summary>
        /// クラスの名前
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }

        /// <summary>
        /// 有効なプラグインを探す
        /// </summary>
        /// <returns>有効なプラグインのPluginInfo配列</returns>
        public static Object GetPluginByName(string PluginName)
        {

            System.Collections.ArrayList plugins =
                new System.Collections.ArrayList();
            //IPlugin型の名前
            string ipluginName = PluginName;// typeof(IInput).FullName;
            
            //プラグインフォルダ
            string folder = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly
                .GetExecutingAssembly().Location);
            folder += "\\plugins";
            if (!System.IO.Directory.Exists(folder))
                throw new ApplicationException(
                    "プラグインフォルダ\"" + folder +
                    "\"が見つかりませんでした。");

            //.dllファイルを探す
            string[] dlls =
                System.IO.Directory.GetFiles(folder, "*.dll");

            foreach (string dll in dlls)
            {
                try
                {
                    //アセンブリとして読み込む
                    System.Reflection.Assembly asm =
                        System.Reflection.Assembly.LoadFrom(dll);
                    foreach (Type t in asm.GetTypes())
                    {
                        //アセンブリ内のすべての型について、
                        //プラグインとして有効か調べる
                        if (t.IsClass && t.IsPublic && !t.IsAbstract &&
                            t.GetInterface(ipluginName) != null)
                        {
                            //PluginInfoをコレクションに追加する
                            plugins.Add(
                                new PluginInfo(dll, t.FullName));
                        }
                    }
                }
                catch
                {
                }
            }
            if (plugins.Count > 0)
            {
                    try
                    {
                        foreach (PluginInfo inf in plugins)
                        {
                            //アセンブリを読み込む
                            System.Reflection.Assembly asm =
                                System.Reflection.Assembly.LoadFrom(inf.Location);
                            //クラス名からインスタンスを作成する
                            return asm.CreateInstance(inf.ClassName);
                        }
                    }
                    catch
                    {
                    }
                    return null;
            }
            else
            {
                return null;
            }
        }
    }
}
