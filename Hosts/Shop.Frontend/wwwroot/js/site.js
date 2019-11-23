﻿window.InitializeDropDowns = function () {
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