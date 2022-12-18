using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
 * UnityでCustom Fontを作る補助をする拡張スクリプト。
 * このスクリプトは、mieki256さんのCharRectSetを基に作られました。
 * https://github.com/mieki256/ChrRectSet
 * 
 * 使い方：（サンプルはFxFPixelFontのファイルを確認してね）
 * ・極小の等幅Pixel FontのPNGをUnityに入れておく。（ex: FxFPFTexture.png）
 * ・Unity > Custom > Custom Font Setting > PNG To CustomFontでPngToCustomFontパネルを開く。
 * ・次の要素を設定する。
 * 　・反映するCustomFontファイル（ないなら新規で作る）
 * 　・Font Texture（さっきのPNG）
 * 　・Grid Layoutの：
 *   　・XとYは0でOK
 *   　・WはPNGの幅
 *   　・HはPNGの高さ
 *   　・Font Count Xは、PNGの中に納められた文字の列数（ex: 32）
 *   　・Font Count Yは、同じく行数（ex: 11）
 *   　・Character Lengthは収録する文字数だけど、単に列×行でもOK（ex: 352）
 *   　・Charactersには、実際に収録する文字を全て入れる。（ex: Characters.txt）
 * ・全ての要素を反映したら、Set Character Rectsを押す。するとフォントファイルに設定が反映されている。
 * ・反映ボタンを押しても、シーン上に配置してるものは更新されないので、手動でtextをいじったりしてね。
 */

/// <summary>
/// PNGからCustomFontを定義する。
/// </summary>
public class PngToCustomFont : EditorWindow {
    public Font customFontObj;
    public TextAsset fontPosTbl;
    public Texture fontTexture;
    public bool xoffsetEnable = true;
    public Vector2 scrollPos;

    public Rect useTexRect = new Rect(0, 0, 256, 256);
    public int fontCountX = 8;
    public int fontCountY = 8;
    public int fontLength = 64;
    public string characters = default;

    /// <summary>
    /// CustomFontにおける文字の画像座標などの情報。
    /// </summary>
    struct ChrRect {
        public int id;
        public int x;
        public int y;
        public int w;
        public int h;
        public int xofs;
        public int yofs;

        public int index;
        public float uvX;
        public float uvY;
        public float uvW;
        public float uvH;
        public int vertX;
        public int vertY;
        public int vertW;
        public int vertH;
        public int advance;
    }

    /// <summary>
    /// Unityのメニューにこいつを追加。
    /// </summary>
    [MenuItem("Custom/Custom Font Setting/PNG To CustomFont")]
    static void Init() {
        EditorWindow.GetWindow(typeof(PngToCustomFont));
    }

    /// <summary>
    /// パネルのレイアウト。
    /// </summary>
    void OnGUI() {
        EditorGUILayout.BeginScrollView(scrollPos);

        customFontObj = (Font)EditorGUILayout.ObjectField("Custom Font", customFontObj, typeof(Font), false);
        fontTexture = (Texture)EditorGUILayout.ObjectField("Font Texture", fontTexture, typeof(Texture), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Grid Layout", EditorStyles.boldLabel);
        useTexRect = EditorGUILayout.RectField("Use Texture Area", useTexRect);
        fontCountX = EditorGUILayout.IntField("Font Count X", fontCountX);
        fontCountY = EditorGUILayout.IntField("Font Count Y", fontCountY);
        fontLength = EditorGUILayout.IntField("Character Length", fontLength);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Characters");
        characters = EditorGUILayout.TextArea(characters);

        if (GUILayout.Button("Set Character Rects"))
        {
            if (customFontObj == null) this.ShowNotification(new GUIContent("No Custom Font selected"));
            else if (fontTexture == null) this.ShowNotification(new GUIContent("No Font Texture selected"));
            else CalcChrRectGrid(customFontObj, fontTexture, useTexRect, fontCountX, fontCountY, Mathf.Min(fontLength, characters.Length));
        }

        EditorGUILayout.EndScrollView();
    }

    void OnInspectorUpdate() {
        this.Repaint();
    }

    /// <summary>
    /// CustomFont情報を準備する。
    /// </summary>
    /// <param name="fontObj"></param>
    /// <param name="tex"></param>
    /// <param name="area"></param>
    /// <param name="xc"></param>
    /// <param name="yc"></param>
    /// <param name="num"></param>
    void CalcChrRectGrid(Font fontObj, Texture tex, Rect area, int xc, int yc, int num) {
        float imgw = (float)tex.width;
        float imgh = (float)tex.height;
        int fw = (int)(area.width - area.x) / xc;
        int fh = (int)(area.height - area.y) / yc;
        List<ChrRect> tblList = new List<ChrRect>();
        for (int i = 0; i < num; i++) {
            int xi = i % xc;
            int yi = i / xc;
            ChrRect d = new ChrRect();
            d.index = characters[i];
            d.uvX = (float)(area.x + (fw * xi)) / imgw;
            d.uvY = (float)(imgh - (area.y + (fh * yi) + fh)) / imgh;
            d.uvW = (float)fw / imgw;
            d.uvH = (float)fh / imgh;
            d.vertX = 0;
            d.vertY = 0;
            d.vertW = fw;
            d.vertH = fh;
            d.advance = fw;
            tblList.Add(d);
        }
        ChrRect[] tbls = tblList.ToArray();
        SetCharacterInfo(tbls, fontObj);
        this.ShowNotification(new GUIContent("Complete"));
    }

    /// <summary>
    /// CustomFontファイルに情報を上書きする。
    /// </summary>
    /// <param name="tbls"></param>
    /// <param name="fontObj"></param>
    void SetCharacterInfo(ChrRect[] tbls, Font fontObj)
    {
        CharacterInfo[] nci = new CharacterInfo[tbls.Length];
        for (int i = 0; i < tbls.Length; i++)
        {
            nci[i].index = tbls[i].index;
            nci[i].advance = tbls[i].advance;

            nci[i].uvBottomLeft = new Vector2(tbls[i].uvX, tbls[i].uvY);
            nci[i].uvBottomRight = new Vector2(tbls[i].uvX + tbls[i].uvW, tbls[i].uvY);
            nci[i].uvTopLeft = new Vector2(tbls[i].uvX, tbls[i].uvY + tbls[i].uvH);
            nci[i].uvTopRight = new Vector2(tbls[i].uvX + tbls[i].uvW, tbls[i].uvY + tbls[i].uvH);
            nci[i].minX = tbls[i].vertX;
            nci[i].minY = -tbls[i].vertH; //高さが負で入る。
            nci[i].maxX = tbls[i].vertW;
            nci[i].maxY = tbls[i].vertY; //0が入る。
        }
        fontObj.characterInfo = nci;
    }
}
