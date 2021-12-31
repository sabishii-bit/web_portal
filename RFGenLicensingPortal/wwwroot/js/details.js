var checkbox = document.getElementById("saasToggle");

window.addEventListener("load", init);

function init() {
  toggleSaasVisibility();
}

const pageTabs = [
  document.querySelector("#nav-tab > li:nth-child(1) > a:nth-child(1)"),
  document.querySelector("#nav-tab > li:nth-child(2) > a:nth-child(1)"),
  document.querySelector("#nav-tab > li:nth-child(3) > a:nth-child(1)"),
  document.querySelector("li.nav-item:nth-child(4) > a:nth-child(1)")
]
const firstElementsOnPage = [null, document.querySelector("#thinClients"), document.querySelector("#salesman"), document.querySelector("#saasToggle")];
const lastElementsOnPage = [document.getElementById("comment"), document.getElementById("vocollectClients"), document.getElementById("server-notes"), null];

function toggleSaasVisibility() {
  if (checkbox.checked) {
    document.getElementById('duration').removeAttribute('readonly');
    document.getElementById('users-allowed').removeAttribute('readonly');
    document.getElementById('server-expiry').removeAttribute('readonly');
    document.getElementById('contract-start').removeAttribute('readonly');
    document.getElementById('contract-end').removeAttribute('readonly');
    document.getElementById('min-user-count').removeAttribute('readonly');
    document.getElementById('heartbeat-date').removeAttribute('readonly');
  }
  else {
    // Set fields to readonly
    document.getElementById('duration').setAttribute('readonly', 'readonly');
    document.getElementById('users-allowed').setAttribute('readonly', 'readonly');
    document.getElementById('server-expiry').setAttribute('readonly', 'readonly');
    document.getElementById('contract-start').setAttribute('readonly', 'readonly');
    document.getElementById('contract-end').setAttribute('readonly', 'readonly');
    document.getElementById('min-user-count').setAttribute('readonly', 'readonly');
    document.getElementById('heartbeat-date').setAttribute('readonly', 'readonly');
    // Clear values from the fields
    document.getElementById('duration').setAttribute('value', '');
    document.getElementById('users-allowed').setAttribute('value', '');
    document.getElementById('server-expiry').setAttribute('value', '');
    document.getElementById('contract-start').setAttribute('value', '');
    document.getElementById('contract-end').setAttribute('value', '');
    document.getElementById('min-user-count').setAttribute('value', '');
    document.getElementById('heartbeat-date').setAttribute('value', '');
  }
}

function tabToNextPage(e) {
  if (!e.shiftKey && e.keyCode == 9) {
    e.preventDefault();
    const currentPage = lastElementsOnPage.indexOf(e.target);
    pageTabs[currentPage + 1].click();
    setTimeout(() => firstElementsOnPage[currentPage + 1].focus(), 500);
  }
}

function tabToPreviousPage(e) {
  if (e.shiftKey && e.keyCode == 9) {
    e.preventDefault();
    const currentPage = firstElementsOnPage.indexOf(e.target);
    pageTabs[currentPage - 1].click();
    setTimeout(() => lastElementsOnPage[currentPage - 1].focus(), 500);
  }
}

checkbox.addEventListener('click', toggleSaasVisibility);
for (element of lastElementsOnPage) {
  if (element != null) {
    element.addEventListener('keydown', tabToNextPage);
  }
}

for (element of firstElementsOnPage) {
  if (element != null) {
    element.addEventListener('keydown', tabToPreviousPage);
  }
}
