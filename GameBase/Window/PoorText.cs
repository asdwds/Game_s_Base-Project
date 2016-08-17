﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CommonPart
{
    /// <summary>
    /// ただの文章stringを、決められたフレーズの前に色指定文字列を追加するなどの処理を行うstring処理用クラス。
    /// </summary>
    class PoorString
    {
        /*
         * プログラム上、colors_list中の0番目から文字列の中に追加される。
         * よって、(前の色の対応string)を含む(対応string)は処理されない。
         * 例; colors_listには[0]"000000",[1]"FFFFFF",
         * colored_strings_listには[0]{"goose"},[1]{"black goose"};
         * PoorStringに渡す文字列文章を"a goose is not a black goose"とする。
         * まず[0]の処理後 "a 000000goose is not a black 000000goose"になる。
         * 次に[1]の処理,　しかし対応の文字列がいないのでそのまま終了。
         * 
         * これの解決方法としては、[0]と[1]の順番を変える=Addの順番を変える。
         * もしくは新しい"FFFFFF"に近い働きをする文字列をcolors_listの[0]にする。
         */
        /// <summary>
        /// 黙認のフォント FontID.Medium
        /// </summary>
        const FontID defaultFont = FontID.Medium;
        // FontID is in TextureManager.cs. directly located at namespace CommonPart
        /// <summary>
        /// 描画する文字列
        /// </summary>
        public readonly string str;

        /*
        /// <summary>
        /// 何も処理していない文字列。
        /// </summary>
        readonly string o_str;
        /// <summary>
        /// 一行に描画できる文字数。この数字ごとに改行が処理中に追加される。
        /// </summary> 
        readonly int max_number_of_chars;
        */

        /// <summary>
        /// 描画用フォント
        /// </summary>
        SpriteFont font;
        
        /// <summary>
        /// 色付けされたい文字列の前に追加処理される  色を指定する文字列。
        /// </summary>
        static List<string> colors_list = new List<string>();
        /// <summary>
        /// このlistのlistに入っているstrは対応したcolors_listの文字列を先頭に追加される。
        /// </summary>
        static List<List<string>> colored_strings_list = new List<List<string>>();

        static PoorString() {
            colors_list.Add("#FFCC00");//orange yellow
            colored_strings_list.Add(new List<string> {"hex"} );

            colors_list.Add("#00AA00");
            colored_strings_list.Add(new List<string> {"virus" });
        }

        public PoorString(string _ostr, int _max_number_of_chars) : this(_ostr,_max_number_of_chars,defaultFont) { }
        public PoorString(string _ostr, int _max_number_of_chars, FontID _fontid)
        {
            str = _ostr;
            font = TextureManager.GetFont(_fontid);

            for (int i = 0; i < colors_list.Count;i++)
            {
                for(int j=0; j < colored_strings_list[i].Count; j++)
                {
                    str.Replace(colored_strings_list[i][j], ");[,%"+i+"'?_{@"+ colored_strings_list[i][j]);
                }
            }
            int k = 0;
            int l = 0; bool inchanging = false;// ");[,%"が発生したならtrueになり、その後"?_{@"が発生したらfalseになる
            while (k < str.Length) {
                if (str[k] == ')'){
                    if (str[k + 1] == ';') {
                        if (str[k + 2] == '[' && str[k + 3] == ',' && str[k + 4] == '%'){
                            k += 5;
                            inchanging = true;
                            continue;
                        }
                    }
                }
                if (inchanging && str[k] == '?')
                {
                    if (str[k + 1] == '_')
                    {
                        if (str[k + 2] == '{' && str[k + 3] == '@')
                        {
                            k += 4;
                            continue;
                        }
                    }
                }
                l++;// count the str[k] as a character.
                if (l >= _max_number_of_chars) { l = 0; str = str.Insert(k, "\n"); }
                k++;
            }
        }
    }
}
