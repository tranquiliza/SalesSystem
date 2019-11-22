window.InitializeDropDowns = function () {
    var options = {
        alignment: 'bottom',
        coverTrigger: false,
        hover: true
    };

    var elems = document.querySelectorAll('.dropdown-trigger');
    var instances = M.Dropdown.init(elems, options);
};