# Make a Me
プレイ動画：https://drive.google.com/drive/folders/18FB3ESFhk0M8HYl5oyPXwcpDE8lD3RUf?usp=drive_link

## ゲーム画面
|プレイ画面|タイトル画面|リザルト画面|
|:----------:|:----------:|:----------:|
|![プレイ画面](./ScreenShot/Play.png)|![タイトル画面](./ScreenShot/Title.png)|![リザルト画面](./ScreenShot/Result.png)|

## ファイル構成
* [Unityデータ](./ProjectData/)
* [ビルドデータ](./BuildData/)

## 概要
|項目|説明|
|:--:|:--|
|**ジャンル**|4人対戦パーティーゲーム|
|**プラットフォーム**|WindowsPC (NintendoSwitchProコントローラーのみ対応)|
|**Unityバージョン**|Unity 2022.3.24f1|
|**制作期間**|2025年9月 約1カ月|
|**制作人数**|5人(プログラマー3人、デザイナー2人)|

## ゲームルール
作業場に入り、自分のロボットを一番多く作るゲーム。  
バッテリーを持つことでエネルギーをチャージし、一定数貯まると自身をレベルアップ。  
レベルアップすることで作業効率を上げたり、バッテリーを相手に投げつけて妨害したりしながら  
ワイワイと楽しめるゲームです。

## こだわり

## 担当プログラム
* [プレイヤーの制御](./ProjectData/Assets/Maeda/Scripts/PlayerController.cs)
  * RotateTowardsメソッドを使い、プレイヤーがなめらかに回転するようにしました。

* [バッテリーの取得制御](./ProjectData/Assets/Maeda/Scripts/TakeRange.cs)
  * hogehoge

* [バッテリーのキャッチ制御](./ProjectData/Assets/Maeda/Scripts/CatchRange.cs)
  *

* [ゲーム進行管理](./ProjectData/Assets/Maeda/Scripts/GameController.cs)
  * コルーチンを使い、ゲームの進行を一括で管理しました。

* [スコア管理](./ProjectData/Assets/Maeda/Scripts/ScoreManager.cs)
  * 構造体を使ってプレイヤーのスコア・順位を管理し、順位に応じて配列の並べ替えを行いました。

* UIアニメーション
  * コルーチンを使い、UIのアニメーション実装しました。
	* [シャッター](./ProjectData/Assets/Maeda/Scripts/ShutterScript.cs)
	* [時計](./ProjectData/Assets/Maeda/Scripts/TimerScript.cs)
	* [カウントダウン](./ProjectData/Assets/Maeda/Scripts/CountDownScript.cs)

* [音源管理]()
  * 音源を一括で管理し、メソッドとして再生・停止を任意のタイミングで呼べるようにしました。
　* AudioMixerを使い、BGM・SEに分けてスレイダーで音量調整できるようにしました。

## 東京ゲームショウ2025出展

## IT未来フェスタ in ResorTech EXPO2025出展

## 制作中の問題とその解決
