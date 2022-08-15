# Up-Stairs-Game

## 筆記
* 跳躍: 判斷能否跳躍的變數，如果直接利用 enterCollision 和 exitCollision 來判斷，以下圖為例，當同時碰撞到兩個物體時，如果不使用跳躍將角色向右側移動，則會觸發到 exitCollision，導致誤以為角色已經離開地面。  

![image](https://user-images.githubusercontent.com/96753049/183745123-f47561b1-801f-4867-921c-b1a881789ca2.png)  

* 解決方法1: 改用 Physics2D.OverlapCapsule(Vector2D, float radius, LayerMask) 進行判斷，以 Vector2D 為圓心，radius 為半徑，尋找範圍內有沒有具備指定 LayerMask 的 collider，回傳的資料型態為 Collider[]，因此可以用 Collider.Length 來判斷是否有範圍內碰撞到物體。

* 解決方法2: 利用 collider.IsTouchingLayers(LayerMask)，判斷當前物件有沒有接觸到其他 collider。  

* 解決方法3: 物件發生碰撞時，collider 會產生兩個連結點，分別是 collider.contacts[0] 和 collider.contacts[1]，可以用 collider.contacts[0].normal 來取得碰撞點跟當前物體本身的法向量，以此判斷碰撞位置是否發生在腳底。  

![image](https://user-images.githubusercontent.com/96753049/183880095-194d18c3-101b-42f9-b851-8f3821344490.png)

以上三種方法都可以修正原本的錯誤，差別在第二種的方式較為簡略，判斷的範圍為物件的 collider 範圍，而如果利用方法 1 和 方法 3 則可以只判斷腳底，其中方法 1 可以用 Vector2 (transform.position.x, transform.position.y - K) 和 radius，來選擇進行判斷的範圍(EX: 腳下)，用方法 1 還有一個好處，如果角色在走斜坡時，理當也要能跳躍，但如果用方法 3，因為是斜坡的關係，碰撞點會來回在腳底跟前進方向間交換，無法精準判斷，利用方法 1 因為查詢範圍是整個圓圈內，因此都能夠正常進行跳躍。

* 如果想改成碰到地面後重製次數，即使沒接觸到地面仍然可以跳躍的話，可以改以下寫法:

<pre><code>if (isGround && rb.velocity.y > 0){
    isGround = false;
}
else if (!isGround && rb.velocity.y <= 0 && Physics2D.OverlapCircle(vector, groundCheckRadius, whatIsGround)){
    isGround = true;
}</code></pre>  
![image](https://user-images.githubusercontent.com/96753049/183751620-d9a14289-1766-4509-99ba-3fd50d9ac8fd.png)

* 跳躍高度: 如果想要根據按下按鈕的時長來改變跳躍高度的話，可以利用 FixedUpdate 和 deltaTime 追蹤當前跳躍時長，可以用以下寫法達成。
<pre><code>void FixedUpdate(){
    CheckGround();
    if (isJumping)
        if (ShouldStopJump())
            StopJump();
        else
            JumpContinuously();
}
bool ShouldStopJump(){
    return (currentJumpDuration < onceJumpMaxDuration) ? false : true;       
}
void JumpContinuously(){
    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    currentJumpDuration += Time.deltaTime;
}</code></pre>  

## 遊戲畫面
![image](https://user-images.githubusercontent.com/96753049/184709216-6bfd95b4-454c-498e-947e-e0b50409f405.png)


![image](https://user-images.githubusercontent.com/96753049/184709017-5a287838-6b13-4a98-870c-b5abc07b566b.png)

