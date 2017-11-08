(function () {
    var myApp = angular.module('MyApp', []);

    myApp.directive('fileUpload', function () {
        return {
            scope: true,        //create a new scope
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var files = event.target.files;
                    //iterate files since 'multiple' may be specified on the element
                    for (var i = 0; i < files.length; i++) {
                        //emit event upward
                        scope.$emit("fileSelected", { file: files[i] });
                    }
                });
            }
        };
    });

    myApp.controller('AddStudentCtrl', ['$http', '$scope', '$timeout', function ($http, $scope, $timeout) {
        $scope.ShowForm = true;
        var Student = {};
        $scope.Warning = false;
        $scope.Success = false;
        $scope.SDate = new Date();
        formData = new FormData();
        $scope.LoadFileData = function (files) {
            for (var file in files) {
                formData.append("file", files[file]);
            }
        };



        $scope.StudentCreate = function () {


            Student = {
                "BDate": $scope.BDate,
                "Name": $scope.Name, "Surname": $scope.Surname,
                "Certificate": $scope.Certificate, "Mark": $scope.Mark,
                "SDate": $scope.SDate,
                "Regimentid": $scope.Regimentid, "Stageid": $scope.Stageid,
                "Centerid": $scope.Centerid, "Sex": $scope.Sex, "FathersName": $scope.FathersName
            };


            formData.append("student", angular.toJson(Student))

            $http.post("/Students/Create", formData, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function successCallback(response) {
                if (response.data) {
                    Student = {};
                    $scope.StCrForm.$setPristine();
                    $scope.StCrForm.$setUntouched();
                    $scope.id = null;
                    $scope.BDate = null;
                    $scope.Name = null;
                    $scope.Surname = null;
                    $scope.Certificate = null;
                    $scope.Mark = null;
                    $scope.State = null;
                    $scope.SDate = null;
                    $scope.EDate = null;
                    $scope.Regimentid = null;
                    $scope.Stageid = null;
                    $scope.Centerid = null;
                    $scope.Sex = null;
                    $scope.FathersName = null;
                    $scope.File = null;
                    $scope.Warning = false;
                    $scope.Success = true;
                    formData.delete('student');
                    $("#imgInp").val(null);
                    $timeout(function () {
                        $scope.Success = false;
                    }, 3000);

                }
                else {
                    Student = {};
                    $scope.Success = false;
                    $scope.Warning = true;

                    formData.delete('student');
                    $("#imgInp").val(null);
                    $timeout(function () {
                        $scope.Warning = false;
                    }, 3000);
                }

            },function errorCallback(response) {
                Student = {};
                $scope.Success = false;
                $scope.Warning = true;

                formData.delete('student');
                $("#imgInp").val(null);
                $timeout(function () {
                    $scope.Warning = false;
                }, 3000);
            });


        };






    }]);


    myApp.controller('AddExamCtrl', ['$http', '$scope', '$timeout', function ($http, $scope, $timeout) {
        $scope.ShowForm = true;
        $scope.Date = new Date();
        $http({ method: 'GET', url: '/Json/FetchStudentsOfDate' }).then(function successCallback(response) {
            $scope.Students = response.data;
        });
        formData = new FormData();

        $scope.LoadFileData = function (files) {
            for (var file in files) {
                formData.append("file", files[file]);
            }
        };
        $scope.Warning = false;
        $scope.Success = false;
        $scope.ExamCreate = function () {
            var Exam = {
                'Desc': $scope.Desc, 'Subjectid': $scope.Subjectid, 'Studentid': $scope.Studentid,
                'ExamTypeid': $scope.ExamTypeid,
                'Mark': $scope.Mark, 'Date': $scope.Date
            };
            formData.append("Exam", angular.toJson(Exam))
                $http.post("/Examinations/Create", formData, {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).then(function successCallback(response) {

                if (response.data) {
                    Exam = {};
                    $scope.ExCrFrom.$setPristine();
                    $scope.ExCrFrom.$setUntouched();
                    $scope.Desc = null;
                    $scope.Subjectid = null;
                    $scope.ExamTypeid = null;
                    $scope.Mark = null;
                    $scope.Date = new Date();
                    $scope.Studentid = null;
                    $scope.Warning = false;
                    $scope.Success = true;

                    formData.delete('Exam');
                    $http({ method: 'GET', url: '/Json/FetchStudentsOfDate' }).then(function successCallback(response) {
                        $scope.Students = response.data;
                    });
                    $("#imgInp").val(null);
                    $timeout(function () {
                        $scope.Success = false;
                    }, 3000);
                }
                else {
                    $scope.Success = false;
                    $scope.Warning = true;

                    formData.delete('Exam');
                    $("#imgInp").val(null);
                    $timeout(function () {
                        $scope.Warning = false;
                    }, 3000);
                }
                    }, function errorCallback(response) {
                        $scope.Success = false;
                        $scope.Warning = true;

                        formData.delete('Exam');
                        $("#imgInp").val(null);
                        $timeout(function () {
                            $scope.Warning = false;
                        }, 3000);
                    });
        };
        $scope.ChangeDate = function () {
            var date = { 'Fielpate': $scope.Date, 'Subjectid': $scope.Subjectid };
            $http({ method: 'Post', url: '/Json/FetchStudentsOfDate', data: date }).then(function successCallback(response) {
                $scope.Students = response.data;
            });
        };





    }]);


    myApp.controller('SearchCtrl', ['$http', '$scope', '$filter', function ($http, $scope) {
        $scope.showButtons = false;
        $scope.ExamSearchBox = function () {
            if ($scope.ExamSearchBoxData || $scope.ExamSearchBoxDate || $scope.ExamSearchBoxNumber) {
                var ToSenh2ext = { 'SearchBoxData': $scope.ExamSearchBoxData, 'SearchBoxDate': $scope.ExamSearchBoxDate, 'SearchBoxNumber': $scope.ExamSearchBoxNumber };
                var myEl = angular.element(document.querySelector('.razorRow'));
                myEl.empty();
                $http({ method: 'POST', url: '/Json/SearchExams', data: ToSenh2ext }).then(function successCallback(response) {
                    $scope.ExamRazorForm = true;
                    $scope.ExamAngularForm = true;
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    });
                });
            }
            else {
                $scope.ExamRazorForm = false;
                $scope.ExamAngularForm = false;
            }
        };

        $scope.StudentSearchBox = function () {
            if ($scope.StudentSearchBoxData || $scope.StudentSearchBoxDate) {
                var ToSenh2ext = { 'SearchBoxData': $scope.StudentSearchBoxData, 'SearchBoxDate': $scope.StudentSearchBoxDate };
                $http({ method: 'POST', url: '/Json/SearchStudents', data: ToSenh2ext }).then(function successCallback(response) {
                    $scope.StudentRazorForm = true;
                    $scope.StudentAngularForm = true;
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        if (value.BDate) {
                            value.BDate = new Date(parseInt(value.BDate.substr(6)));
                        }
                        if (value.EDate) {
                            value.EDate = new Date(parseInt(value.EDate.substr(6)));
                        }
                        if (value.SDate) {
                            value.SDate = new Date(parseInt(value.SDate.substr(6)));
                        }
                    });
                });
            }
            else {
                $scope.StudentRazorForm = false;
                $scope.StudentAngularForm = false;
            }

        };

        $scope.PresenceSearchBox = function () {
            if ($scope.PresenceSearchBoxData || $scope.PresenceSearchBoxDate) {
                var ToSenh2ext = { 'SearchBoxData': $scope.PresenceSearchBoxData, 'SearchBoxDate': $scope.PresenceSearchBoxDate };
                $http({ method: 'POST', url: '/Json/SearchPresence', data: ToSenh2ext }).then(function successCallback(response) {
                    $scope.PresenceRazorForm = true;
                    $scope.PresenceAngularForm = true;
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    });
                });
            }
            else {
                $scope.PresenceRazorForm = false;
                $scope.PresenceAngularForm = false;
            }
        };



        $scope.EmployeeSearchBox = function () {
            if ($scope.EmployeeSearchBoxData || $scope.EmployeeSearchBoxDate) {
                var ToSenh2ext = { 'SearchBoxData': $scope.EmployeeSearchBoxData, 'SearchBoxDate': $scope.EmployeeSearchBoxDate };
                $http({ method: 'POST', url: '/Json/SearchEmployees', data: ToSenh2ext }).then(function successCallback(response) {
                    $scope.EmployeeRazorForm = true;
                    $scope.EmployeeAngularForm = true;
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        if (value.EmployeeBDate) {
                            value.EmployeeBDate = new Date(parseInt(value.EmployeeBDate.substr(6)));
                        }
                        if (value.EmployeeSDate) {
                            value.EmployeeSDate = new Date(parseInt(value.EmployeeSDate.substr(6)));
                        }
                        if (value.EmployeeEDate) {
                            value.EmployeeEDate = new Date(parseInt(value.EmployeeEDate.substr(6)));
                        }

                    });
                });
            }
            else {
                $scope.EmployeeRazorForm = false;
                $scope.EmployeeAngularForm = false;
            }

        };







    }]);

    myApp.controller('OrdersCtrl', ['$http', '$scope', '$filter', function ($http, $scope) {
        $http.get("/Orders/GetOrders")
            .then(function (response) {
                $scope.Orders = response.data[0];
                $scope.empid = response.data[1];
                
                $scope.Orders.Date = new Date($scope.Orders.Date);

                angular.forEach($scope.Orders, function (value, key) {
                    if (value.Date) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    }


                });
            });


    }]);

})();
