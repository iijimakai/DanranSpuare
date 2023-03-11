# DanranSpuare

## Unityバージョン

・2021.3.20f1

## 使用ツール

・UniRx

・UniTask

・Addressable

・DOTweenPro

## コーディングルール

### 命名規則

#### 変数

キャメルケース

attributeを入れる

get/setアクセサを使うがsetはprivateを宣言する

例

[SerierizeField, Header("変数の上に出る説明"), ToolTip("カーソルを合わせた時に出る"), Range(0,10)]

public int temp {get; private set;} = 10;

#### 関数

パスカルケース

関数に\<summary>を入れる

\<param name = "">や\<return>の記述

例
/// \<summary>

/// isNewGameのセッター

/// \</summary>

/// \<param name="temp">引数の役割</param>

public void Test(int temp)

{

}

#### クラス名

パスカルケース

例

public class TestScripts 

## ネームスペース

基本入れる

システム名

パスカルケース

例

namespace Player

namespace Enemy

##git運用

自身のブランチを切る

プルリクエストを出したらSlackにメンションをつけて通知
