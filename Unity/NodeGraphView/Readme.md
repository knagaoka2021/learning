
# NodeGraphView

## Image
![screenshot_01](https://user-images.githubusercontent.com/86211525/126427109-a394499a-2a7d-4d2e-8b53-7812a3bd2e79.png)
![image](https://user-images.githubusercontent.com/86211525/127448605-1c3f0fbe-ebd7-4818-bc52-2352c5aac345.png)

# Info
Unityエディタ拡張 GraphView機能を用いたノードエディタ  

#### ツール操作
- UnityEditor 開く  
  →Unity上部 Windowタブ→Open NodeGraphView  
  Or 作製したAssetファイルをダブルクリックで開く  
  ※サンプル  
    Assets/Editor/Resources/NodeGraph/サンプル用01.asset  
    Assets/Editor/Resources/NodeGraph/サンプル用02.asset  
    Assets/Editor/Resources/NodeGraph/サンプル用03.asset  
- 右クリック  
  - Create Node  
  → SelectorNode,SequenceNode,ActionNodeをクリックでEditor上に作成する  
- ノード選択(ドラッグによる範囲選択可能) & 右クリック  
  - Copy  
  → ノードをコピーする  
  - Paste  
  → コピーしたノードをEditor上に作成する  
  - Duplicate  
  → ノードに接続されたエッジ(線)を削除する  
  - Delete  
  → ノードを削除する  

  
#### UIボタン機能
- Save  
  →現在のノードデータをAssetファイルに書き込みしてAssetファイルを作成する  
  `Assets/Editor/Resources/NodeGraph/NodeGraphView_YYYYMMDD_HHMMSS.asset`
- Load  
  →Assetファイルを読み込んでノードデータを再作成する
- Clear  
  →Editor上のノードをすべて削除する
- Invoke  
  →子ノードを順番実行する  

#### ■ノード機能
- Rootノード  
→処理開始するためのノード
- Selectorノード  
下位ノードをindex昇順に実行していき途中で1つでも評価が失敗の場合、評価結果を返却する
- Sequenceノード  
下位ノードをindex昇順に実行していき途中で1つでも評価が成功の場合、評価結果を返却する
- Actionノード  
下位ノードであり３種類の評価をして評価結果を返却する。  
  - 時間待機
  - 距離判定
  - ステータス割合、大小判定

#### ■Action評価機能
##### 振る舞い種別で下記を指定する。  
- 時間待機  
  →n秒間待機 ※実行後は必ず成功を返却  
- 距離判定  
  →ゼロ地点と指定座標の距離の長さの大小評価  
  - ※指定座標はダミーデータ(MOVE)で指定
- ステータス割合、大小判定  
  →対象ステータスの割合を評価する。
    - ※対象ステータス値はダミーデータ(STATUS)で指定

## SCREENSHOT
![image](https://user-images.githubusercontent.com/86211525/127448605-1c3f0fbe-ebd7-4818-bc52-2352c5aac345.png)
![image](https://user-images.githubusercontent.com/86211525/127448573-6213d78b-c7d4-4040-a66a-ed5c269632c6.png)
![image](https://user-images.githubusercontent.com/86211525/127448641-6c90fd5d-c578-439c-9685-48605d2808d7.png)
![image](https://user-images.githubusercontent.com/86211525/127448661-9fed56f3-b70b-4f08-8c3c-ce3d49e9bfa3.png)
