// Javascript for tabs within a page. See use on Administration page.
//
// Instructions:
// 1) Add div with button for each tab.
// 2) Add div for each tab with content.
function openPageTab(linkId, tabId) {
    // Get all elements with class="pagetabcontent" and hide them
    let tabcontent = document.getElementsByClassName("pagetabcontent");
    for (let i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="pagetablinks" and remove the class "active"
    let tablinks = document.getElementsByClassName("pagetablinks");
    for (let i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the link that opened the tab
    document.getElementById(tabId).style.display = "block";
    document.getElementById(linkId).className += " active";
}