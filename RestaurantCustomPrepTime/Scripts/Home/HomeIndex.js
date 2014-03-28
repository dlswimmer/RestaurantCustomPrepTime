
$(document).ready(function () {
    ko.validation.configure({ messagesOnModified: false, insertMessages: true });
    ko.validation.init();
    ko.applyBindings(viewModel);
    viewModel().load();

    $('#addEditDialog').dialog({
        autoOpen: false,
        width: 700,
        modal: true,
        resizable: false
    });
});

var viewModel = ko.validatedObservable({
    prepTimes: ko.observableArray([]),
    selectedPrepTime: ko.validatedObservable(),
    availableDays: [
        { name: "Mon", value: "1" },
        { name: "Tue", value: "2" },
        { name: "Wed", value: "4" },
        { name: "Thu", value: "8" },
        { name: "Fri", value: "16" },
        { name: "Sat", value: "32" },
        { name: "Sun", value: "64" }],
    load: function () {
        var self = this;
        $.ajax({
            type: "GET",
            url: window.rootHomeGetAllUri,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.prepTimes($.map(data, function (item) {
                    return new PrepTimes(item);
                }));
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    },
    addNew: function () {
        var item = ko.validatedObservable(new PrepTimes());
        item().editSelf();
    }
});

function convert(daysInt) {
    var strArr = new Array();
    for (i in daysInt) {
        var str = daysInt[i].toString();
        strArr.push(str);
    }
    return strArr;
}

function PrepTimes(data) {
    var self = this;
    self.Id = ko.observable(data == null ? 0 : data.Id);
    self.Days = ko.observableArray(data == null ? false : convert(data.Days));
    self.TimeFrom = ko.observable(data == null ? "" : data.TimeFrom).extend({
        required: {
            message: "*",
            params: true
        },
        pattern: {
            message: "Please enter in the following format: 12:00 PM",
            params: "^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d) )?(AM|PM)$"
        },
        rateLimit: { timeout: 600, method: "notifyWhenChangesStop" }
    });
    self.TimeTo = ko.observable(data == null ? "" : data.TimeTo).extend({
        required: {
            message: "*",
            params: true
        },
        pattern: {
            message: "Please enter in the following format: 12:00 PM",
            params: "^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d) )?(AM|PM)$"
        },
        rateLimit: { timeout: 600, method: "notifyWhenChangesStop" }
    });
    self.PrepTime = ko.observable(data == null ? "" : data.PrepTime).extend({
        required: {
            message: "*",
            params: true
        },
        min: 1,
        rateLimit: { timeout: 600, method: "notifyWhenChangesStop" }
    });
    self.isNew = ko.computed(function() {
        return self.Id() == null || self.Id() == 0;
    });
    self.times = ko.computed(function () {
        return self.TimeFrom() + " - " + self.TimeTo();
    });
    self.daysDisplay = ko.computed(function () {
        var result = "";
        var last = "";
        var leng = 0;
        var addOrPush = function (val, array, text) {
            if ($.inArray(val, array) >= 0) {
                last = text;
                if (leng == 0) {
                    if (result.length > 0) {
                        result += ", ";
                    }
                    result += text;
                }
                leng++;
            } else if (leng > 0) {
                if (leng > 1) {
                    result += " - " + last;
                }
                leng = 0;
                last = "";
            }
        };

        for (i in viewModel().availableDays) {
            var day = viewModel().availableDays[i];
            addOrPush(day.value, self.Days(), day.name);
        }
        addOrPush("null", self.Days(), "");
        return result;
    });
    self.deleteSelf = function () {
        $.ajax({
            type: "POST",
            url: window.rootHomeDeleteUri + "/" + self.Id(),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                viewModel().prepTimes.remove(self);
            },
            error: function (err) {
                alert(err.status + " : " + err.statusText);
            }
        });
    };
    self.editSelf = function () {
        var copy = ko.validatedObservable(new PrepTimes());
        copy().copyValues(self);
        viewModel().selectedPrepTime(copy);
        $("#addEditDialog").dialog('open');
    };
    self.save = function () {
        if (!self.isValid()) {
            return;
        }
        var item = self.clean();
        if (self.isNew()) {
            $.ajax({
                type: "POST",
                url: window.rootHomeAddUri,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: item,
                success: function (jsonData) {
                    var result = new PrepTimes(jsonData);
                    viewModel().prepTimes.push(result);
                    self.closeForm();
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }
            });
        } else {
            $.ajax({
                type: "POST",
                url: window.rootHomeEditUri,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: item,
                success: function (jsonData) {
                    var result = new PrepTimes(jsonData);
                    self.copyFrom.copyValues(result);
                    self.closeForm();
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }
            });
        }
    };
    self.copyValues = function (copyFrom) {
        self.copyFrom = copyFrom;
        self.Id(copyFrom.Id());
        self.Days(copyFrom.Days().slice());
        self.TimeFrom(copyFrom.TimeFrom());
        self.TimeTo(copyFrom.TimeTo());
        self.PrepTime(copyFrom.PrepTime());
    };
    self.clean = function() {
        var result = {
            Id: self.Id(),
            Days: self.Days().slice(),
            TimeFrom: self.TimeFrom(),
            TimeTo: self.TimeTo(),
            PrepTime: self.PrepTime()
        }
        return ko.toJSON(result);
    }
    self.closeForm = function () {
        $("#addEditDialog").dialog('close');
    };
}