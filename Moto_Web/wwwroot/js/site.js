// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//function FillModels(lstCompanyCtrl, lstModelId) {

//    var lstModels = $("#" + lstModelId);
//    lstModels.empty();

//    var selectedCompany = lstCompanyCtrl.options[lstCompanyCtrl.selectedIndex].value;

//    if (selectedCompany != null && selectedCompany != '') {
//        $.getJSON("/ads/GetModelsByCompany", { companyId: selectedCompany }, function (models) {
//            if (models != null && !jQuery.isEmptyObject(models)) {
//                $.each(models, function (index, city) {
//                    lstModels.append($('<option/>',
//                        {
//                            value: city.value,
//                            text: city.text
//                        }));
//                });
//            };

//    }
//    return;
//}