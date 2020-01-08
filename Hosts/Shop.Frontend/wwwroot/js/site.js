window.InitializeDropDowns = function () {
    var options = {
        alignment: 'bottom',
        coverTrigger: false,
        hover: true
    };

    var elems = document.querySelectorAll('.dropdown-trigger');
    var instances = M.Dropdown.init(elems, options);
};

window.InitializeSideNav = function () {
    var elems = document.querySelectorAll('.sidenav');
    var instances = M.Sidenav.init(elems, {});
};

window.InitializeCollapsibles = function () {
    var elems = document.querySelectorAll('.collapsible');
    var instances = M.Collapsible.init(elems, {});
};

window.FixedActionButton = function () {
    var elems = document.querySelectorAll('.fixed-action-btn');
    var instances = M.FloatingActionButton.init(elems, {});
};

window.RefreshInputFields = function () {
    M.updateTextFields();
};

window.InitializeTabs = function () {
    var el = document.querySelector('#basketProgressBar');
    var basketProgressBar = M.Tabs.init(el, {});
};

window.UpdateTabIndicator = function () {
    let el = document.querySelector('#basketProgressBar');
    let instance = M.Tabs.getInstance(el);
    instance.updateTabIndicator();
};

window.BasketSwitchToPayment = function () {
    let tab = document.querySelector('#basketPaymentTab');
    tab.classList.remove("disabled");

    let el = document.querySelector('#basketProgressBar');
    let instance = M.Tabs.getInstance(el);
    instance.select('Payment');
};

window.DisablePaymentTab = function () {
    let tab = document.querySelector('#basketPaymentTab');
    tab.classList.add("disabled");
};

window.InitializeModals = function () {
    var elems = document.querySelectorAll('.modal');
    var instances = M.Modal.init(elems, {});
};

window.InitializeImageBoxes = function () {
    var elems = document.querySelectorAll('.materialboxed');
    var instances = M.Materialbox.init(elems, {});
};

window.CloseModal = function (elementId) {
    let el = document.querySelector('#' + elementId);
    var instance = M.Modal.getInstance(el);

    instance.close();
};

window.TranquilizaSetItem = function (key, value) {
    this.localStorage.setItem(key, value);
};

window.TranquilizaGetItem = function (key) {
    return this.localStorage.getItem(key);
};