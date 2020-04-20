
/**
 * 替換topbutton 按鈕顯示的名稱
 * @param {any} buttonId 
 * @param {any} text
 */
function replaceButtonText(buttonId, text) {
    if (document.getElementById) {
        var button = document.getElementById(buttonId);
        if (button) {
            if (button.childNodes[0]) {
                button.childNodes[0].nodeValue = text;
            }
            else if (button.value) {
                button.value = text;
            }
            else //if (button.innerHTML)
            {
                button.innerHTML = text;
            }
        }
    }
}