
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

function PrepTimes(data) {
    var self = this;
    self.Id = ko.observable(data == null ? 0 : data.Id);
    self.OnMonday = ko.observable(data == null ? false : data.OnMonday);
    self.OnTuesday = ko.observable(data == null ? false : data.OnTuesday);
    self.OnWednesday = ko.observable(data == null ? false : data.OnWednesday);
    self.OnThursday = ko.observable(data == null ? false : data.OnThursday);
    self.OnFriday = ko.observable(data == null ? false : data.OnFriday);
    self.OnSaturday = ko.observable(data == null ? false : data.OnSaturday);
    self.OnSunday = ko.observable(data == null ? false : data.OnSunday);
    self.TimeFrom = ko.observable(data == null ? "" : data.TimeFrom).extend({
        required: {
            message: "*",
            params: true
        },
        pattern: {
            message: "Please enter in the following format: 12:00 PM",
            params: "^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d) )?(AM|PM)$"
        }
    });
    self.TimeTo = ko.observable(data == null ? "" : data.TimeTo).extend({
        required: {
            message: "*",
            params: true
        },
        pattern: {
            message: "Please enter in the following format: 12:00 PM",
            params: "^(?:(?:([01]?\\d|2[0-3]):)?([0-5]?\\d) )?(AM|PM)$"
        }
    });
    self.PrepTime = ko.observable(data == null ? "" : data.PrepTime).extend({
        required: {
            message: "*",
            params: true
        }, min: 1
    });
    self.isNew = ko.computed(function() {
        return self.Id() == null || self.Id() == 0;
    });
    self.times = ko.computed(function () {
        return self.TimeFrom() + " - " + self.TimeTo();
    });
    self.days = ko.computed(function () {
        var arr = [];
        var current = [];
        arr.push(current);
        if (self.OnMonday()) {
            current.push("Mon");
        }
        var addOrPush = function (isTrue, val) {
            if (isTrue) {
                current.push(val);
            } else if (current.length > 0) {
                current = [];
                arr.push(current);
            }
        };
        addOrPush(self.OnTuesday(), "Tue");
        addOrPush(self.OnWednesday(), "Wed");
        addOrPush(self.OnThursday(), "Thu");
        addOrPush(self.OnFriday(), "Fri");
        addOrPush(self.OnSaturday(), "Sat");
        addOrPush(self.OnSunday(), "Sun");

        var result = "";
        for (var i = 0; i < arr.length; i++) {
            var item = arr[i];
            if (item.length == 0) {
                break;
            }
            if (i > 0) {
                result += ", ";
            }
            if (item.length > 1) {
                result += item[0] + " - " + item[item.length - 1];
            } else {
                result += item[0];
            }
        }
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
        var item = ko.toJSON(self);
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
                    $("#addEditDialog").dialog('close');
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
                    $("#addEditDialog").dialog('close');
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
        self.OnMonday(copyFrom.OnMonday());
        self.OnTuesday(copyFrom.OnTuesday());
        self.OnWednesday(copyFrom.OnWednesday());
        self.OnThursday(copyFrom.OnThursday());
        self.OnFriday(copyFrom.OnFriday());
        self.OnSaturday(copyFrom.OnSaturday());
        self.OnSunday(copyFrom.OnSunday());
        self.TimeFrom(copyFrom.TimeFrom());
        self.TimeTo(copyFrom.TimeTo());
        self.PrepTime(copyFrom.PrepTime());
    };
    self.closeForm = function () {
        $("#addEditDialog").dialog('close');
    };
}