
function createSubDiv(i, len, DivName) {
    var getDiv = document.getElementById(DivName);
    var subDiv = document.createElement("DIV");
    var btnHR = document.createElement("HR");

    subDiv.id = "div" + i;
    subDiv.style.height = "60px";
    subDiv.style.width = "270px";
    //console.log("subDiv.id =" + subDiv.id);
    btnHR.className = "hrSt";

    getDiv.appendChild(subDiv);
    getDiv.appendChild(btnHR);  
}


function createBleButton(i, bleid, blesn, bleName, controller) {
    //alert("createButton here");
    var btn = document.createElement("BUTTON");
    var btnRenameEditor = document.createElement("BUTTON");
    var btnSearch = document.createElement("BUTTON");
    var k = 1;
    var subDivId = "div" + i;
    console.log("2cr subDiv.id =" + subDivId);
    var getSubDiv = document.getElementById(subDivId);

    //console.log("cb bleId = " + bleId);
    //console.log("cb sn = " + sn);
    //console.log("cb bleName = " + bleName);

    btn.id = "blebtn_" + i;
    btn.innerHTML = "btn_" + i;
    var indx = i + 1;

    btn.innerHTML = "<span style=''>" + bleName + "&nbsp0" + i + "</span><br /><span class='DeviceIDSNBlock'> ID: " + bleid + "<br />SN:" + blesn + "</span><br />";
    btn.style.height = "19px";
    btn.style.width = "180px";
    btn.className = "boxSt";
    $(btn).attr('data-bleID', bleid);
    $(btn).attr('data-bleSN', blesn);
    $(btn).attr('data-bleIDSN', bleid + blesn);

    btnRenameEditor.id = "bleEditBtn_" + i;
    btnRenameEditor.style.height = "40px";
    btnRenameEditor.style.width = "40px";
    btnRenameEditor.className = "btnRenameEdit";

    btnSearch.id = "bleSearchBtn_" + i;
    btnSearch.style.height = "40px";
    btnSearch.style.width = "35px";
    btnSearch.className = "btnSearch";

    // 3. Add event handler
    btn.addEventListener("click", function () {
        //alert(btn.id);
        var controllerDevicePanel = controller + "DeviceKeyPanel"; 
        var controllerDeviceAction = "Selected" + controller + "Device"; //GetSelectedMAXLite3Device
        var params = "?BleID=" + bleid + "&BleSN=" + blesn ;
        //var uri = "/" + controllerDevicePanel + "/" + controllerDeviceAction + params;
        var uri = "/" + controllerDevicePanel + "/"
        console.log("uri = " + uri);
        //var method = "SelectedMAXLite1Device";
        var method = "Selected" + controller + "Device";
        var methodUri = "/" + controllerDevicePanel + "/" + method;
        //alert("methodUri = " + methodUri);
        console.log("methodUri = " + methodUri);
        //window.location.href = uri;
        $.ajax({
            type: "POST",
            url: methodUri,
            data: { 'BleID': bleid, 'BleSN': blesn },
            dataType: "html",
            cache: false,
            success: function (result, status, xhr) {
                //$("#dataDiv").html(result);
                //alert(controller + " update status: " + result);
                window.location.href = uri;
            },
            error: function (xhr, status, error) {
                //alert(controller + " update status error: " + status + " " + error + " " + xhr.status + " " + xhr.statusText);
                //$("#dataDiv").html("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
            }
        });
        return false;
    });

    // 4. Add event handler
    btnRenameEditor.addEventListener("click", function () {
        ////alert(btnRenameEditor.id);
        $('#DeviceID').val(bleid);
        $('#DeviceSN').val(blesn);
        $('#OldDeviceName').val(bleName);

        $('#hidSelectedDeviceID').val(bleid);
        $('#hidSelectedDeviceSN').val(blesn);
        $('#hidSelectedDeviceIDSN').val(bleid + blesn);
        $('#hidSelectedDeviceOldName').val(bleName);

        $("#RenameDialog").dialog("open");
    });

    // 4. Add event handler
    btnSearch.addEventListener("click", function () {
        alert(btnSearch.id);
    });

    getSubDiv.style.height = "42px";
    getSubDiv.style.width = "285px";
    getSubDiv.appendChild(btn);
    getSubDiv.appendChild(btnSearch);
    getSubDiv.appendChild(btnRenameEditor);

}  

function UpdateDeviceAjaxCall(controller, method, bleId, sn, newBleName) {
    //alert("UpdateDeviceAjaxCall !!");
    var methodUri = "/" + controller + "/" + method;

    console.log("methodUri = " + methodUri);
    console.log("methodUri bleId= " + bleId);
    console.log("methodUri sn= " + sn);
    console.log("methodUri newBleName= " + newBleName);
    
    $.ajax({
        type: "POST",
        url: methodUri,
        data: { 'BleID': bleId, 'BleSN': sn, 'BleNameUpdate': newBleName },
        dataType: "html",
        cache: false,
        success: function (result, status, xhr) {
            //$("#dataDiv").html(result);
            //alert(controller + " update status: " + result);
        },
        error: function (xhr, status, error) {
            //alert(controller + " update status error: " + status + " " + error + " " + xhr.status + " " + xhr.statusText);
            //$("#dataDiv").html("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
        }
    });
    return false;
}

function loadLocalJsonData(loadingdiv, DeviceBleID) {
    //read local json file
    var jqxhr = $.getJSON("/Helper/blelist.json", function (data) {
        console.log("load blefile success");
    })
        .done(function () {
            console.log("load blefile second success");
        })
        .fail(function () {
            console.log("load blefile error");
        })
        .always(function (data) {
            console.log("load blefile complete");
            console.log(data);
            bleJsonData = data;
            //console.log("bleJsonData = " + bleJsonData);

            for (var i = 0; i < data.length; ++i) {
                var bleid = data[i].BleID;
                if (bleid == DeviceBleID) {
                    createSubDiv(i, data.length, loadingdiv);
                }
            }

            for (var i = 0; i < data.length; ++i) {
                var pid = data[i].PID;
                var bleid = data[i].BleID;
                var blesn = data[i].BleSN;
                var blename = data[i].BleName;

                //console.log("cb i pid = " + i + "--" + pid);
                //console.log("cb bleId = " + bleid);
                //console.log("cb sn = " + blesn);
                //console.log("cb bleName = " + blename);

                if (bleid == DeviceBleID) {
                    createButton(i, pid, bleid, blesn, blename);
                }
            }
        });
}


