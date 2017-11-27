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
                "Centerid": $scope.Centerid, "Sex": $scope.Sex, "FathersName": $scope.FathersName, "OldSchool": $scope.OldSchool,
                "Mothersname": $scope.Mothersname, "StudentState": $scope.StudentState
            };


            formData.append("student", angular.toJson(Student));

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
                    $scope.Mothersname = null;
                    $scope.OldSchool = null;
                    $scope.StudentState = null;
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

            }, function errorCallback(response) {
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
            formData.append("Exam", angular.toJson(Exam));
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

    myApp.controller('MissionsCtrl', ['$http', '$scope', function ($http, $scope) {
        $scope.timeFunction = function (timeObj) {
            if (timeObj != null) {
                var min = timeObj.Minutes < 10 ? "0" + timeObj.Minutes : timeObj.Minutes;
                var hour = timeObj.Hours < 10 ? "0" + timeObj.Hours : timeObj.Hours;
                return hour + ':' + min;
            }
            return null;
        };

        $scope.CheckMission = function (id) {
            var data = { id };
            $http({ method: 'POST', url: '/Json/CheckMission', data }).then(function successCallback(response) {
                if (response.data == true) {
                    $scope.MissionIsClosed = true;
                }
            });

        };

        $scope.CloseMission = function (id) {
            var data = { id };
            $http({ method: 'POST', url: '/Json/CloseMission', data }).then(function successCallback(response) {
                if (response.data == true) {
                    $scope.MissionIsClosed = true;
                }
            });

        };

        $http({ method: 'GET', url: '/Json/Missions' }).then(function successCallback(response) {
            $scope.Missions = response.data[0];
            $scope.Responses = response.data[1];
            $scope.empid = response.data[2];
            angular.forEach($scope.Missions, function (value, key) {
                if (value.DateOfEntry) {
                    value.DateOfEntry = new Date(parseInt(value.DateOfEntry.substr(6)));
                }
                if (value.DateOfFinish) {
                    value.DateOfFinish = new Date(parseInt(value.DateOfFinish.substr(6)));
                }
                if (value.DateOfLastModification) {
                    value.DateOfLastModification = new Date(parseInt(value.DateOfLastModification.substr(6)));
                }
            });
            $('#LoadingScreen').hide();

        });

            $scope.ShowHistory = function () {
                $('#LoadingScreen').show();
                $http({ method: 'GET', url: '/Json/MissionsShowHistory' }).then(function successCallback(response) {
                    $scope.Missions = response.data[0];
                    $scope.Responses = response.data[1];
                    $scope.empid = response.data[2];
                    angular.forEach($scope.Missions, function (value, key) {
                        if (value.DateOfEntry) {
                            value.DateOfEntry = new Date(parseInt(value.DateOfEntry.substr(6)));
                        }
                        if (value.DateOfFinish) {
                            value.DateOfFinish = new Date(parseInt(value.DateOfFinish.substr(6)));
                        }
                        if (value.DateOfLastModification) {
                            value.DateOfLastModification = new Date(parseInt(value.DateOfLastModification.substr(6)));
                        }
                    });
                    $scope.OpenMisisons = true;
                    $('#LoadingScreen').hide();
                });
        };

            $scope.ShowNonDone = function () {
                $('#LoadingScreen').show();
                $http({ method: 'GET', url: '/Json/Missions' }).then(function successCallback(response) {
                    $scope.Missions = response.data[0];
                    $scope.Responses = response.data[1];
                    $scope.empid = response.data[2];
                    angular.forEach($scope.Missions, function (value, key) {
                        if (value.DateOfEntry) {
                            value.DateOfEntry = new Date(parseInt(value.DateOfEntry.substr(6)));
                        }
                        if (value.DateOfFinish) {
                            value.DateOfFinish = new Date(parseInt(value.DateOfFinish.substr(6)));
                        }
                        if (value.DateOfLastModification) {
                            value.DateOfLastModification = new Date(parseInt(value.DateOfLastModification.substr(6)));
                        }
                    });
                    
                    $scope.OpenMisisons = false;
                    $('#LoadingScreen').hide();
                });
            };
    }]);


    myApp.controller('StudentsCtrl', ['$http', '$scope', function ($http, $scope) {

        $http({ method: 'GET', url: '/Json/SearchStudents' }).then(function successCallback(response) {
            $scope.Students = response.data;
            angular.forEach($scope.Students, function (value, key) {
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
            $('#LoadingScreen').hide();
        });

        $scope.StudentSearchBox = function () {
            $('#LoadingScreen').show();
            if ($scope.StudentSearchBoxData || $scope.StudentSearchBoxDate) {
                var ToSenh2ext = { 'SearchBoxData': $scope.StudentSearchBoxData, 'SearchBoxDate': $scope.StudentSearchBoxDate };
                $http({ method: 'POST', url: '/Json/SearchStudents', data: ToSenh2ext }).then(function successCallback(response) {
                    $scope.Students = response.data;
                    angular.forEach($scope.Students, function (value, key) {
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
                    $('#LoadingScreen').hide();
                });
            }
            else {
                $http({ method: 'GET', url: '/Json/SearchStudents' }).then(function successCallback(response) {
                    $scope.Students = response.data;
                    angular.forEach($scope.Students, function (value, key) {
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
                    $('#LoadingScreen').hide();
                });
            }

        };


    }]);


    myApp.controller('PresencesCtrl', ['$http', '$scope', function ($http, $scope) {

        $http({ method: 'GET', url: '/Json/Presences' }).then(function successCallback(response) {
            $scope.Exams = response.data;
            angular.forEach($scope.Exams, function (value, key) {
                value.Date = new Date(parseInt(value.Date.substr(6)));
            });
            $('#LoadingScreen').hide();
        });
        $scope.PresenceSearchBox = function () {
            $('#LoadingScreen').show();
            if ($scope.PresenceSearchBoxData || $scope.PresenceSearchBoxDate) {

                var ToSenh2ext = { 'SearchBoxData': $scope.PresenceSearchBoxData, 'SearchBoxDate': $scope.PresenceSearchBoxDate };
                $http({ method: 'POST', url: '/Json/Presences', data: ToSenh2ext }).then(function successCallback(response) {
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    });
                    $('#LoadingScreen').hide();
                });
            }
            else {
                $http({ method: 'GET', url: '/Json/Presences' }).then(function successCallback(response) {
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    });
                    $('#LoadingScreen').hide();
                });
            }

        };

    }]);


    myApp.controller('EmployeesCtrl', ['$http', '$scope', function ($http, $scope) {

        $http({ method: 'GET', url: '/Json/Employees' }).then(function successCallback(response) {
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
            $('#LoadingScreen').hide();

        });

        $scope.EmployeeSearchBox = function () {
            $('#LoadingScreen').show();
            if ($scope.EmployeeSearchBoxData || $scope.EmployeeSearchBoxDate) {
                var ToSenh2ext = { 'SearchBoxData': $scope.EmployeeSearchBoxData, 'SearchBoxDate': $scope.EmployeeSearchBoxDate };
                $http({ method: 'POST', url: '/Json/Employees', data: ToSenh2ext }).then(function successCallback(response) {
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
                    $('#LoadingScreen').hide();
                });
            }
            else {
                $http({ method: 'GET', url: '/Json/Employees' }).then(function successCallback(response) {
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
                    $('#LoadingScreen').hide();
                });            }

        };


    }]);






    myApp.controller('ExamsCtrl', ['$http', '$scope', '$filter', function ($http, $scope) {
        $http({ method: 'GET', url: '/Json/Exams' }).then(function successCallback(response) {
            $scope.Exams = response.data;
            angular.forEach($scope.Exams, function (value, key) {
                value.Date = new Date(parseInt(value.Date.substr(6)));
            });
            $('#LoadingScreen').hide();
        });

        $scope.ExamSearchBox = function () {
            $('#LoadingScreen').show();
            if ($scope.ExamSearchBoxData || $scope.ExamSearchBoxDate || $scope.ExamSearchBoxNumber) {
                var ToSenh2ext = { 'SearchBoxData': $scope.ExamSearchBoxData, 'SearchBoxDate': $scope.ExamSearchBoxDate, 'SearchBoxNumber': $scope.ExamSearchBoxNumber };
                var myEl = angular.element(document.querySelector('.razorRow'));
                myEl.empty();
                $http({ method: 'POST', url: '/Json/Exams', data: ToSenh2ext }).then(function successCallback(response) {
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    });
                    $('#LoadingScreen').hide();
                });
            }
            else {
                $http({ method: 'GET', url: '/Json/Employees' }).then(function successCallback(response) {
                    $scope.Exams = response.data;
                    angular.forEach($scope.Exams, function (value, key) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    });
                    $('#LoadingScreen').hide();
                });
            }
        };
    }]);

    myApp.controller('OrdersCtrl', ['$http', '$scope', '$window', function ($http, $scope, $window) {
        formData = new FormData();
        $http.get("/Orders/GetOrders")
            .then(function (response) {
                $scope.Orders = response.data[0];
                $scope.empid = response.data[1];
                $scope.Bnds = response.data[2];
                $scope.Payments = response.data[3];
                $scope.Orders.Date = new Date($scope.Orders.Date);

                angular.forEach($scope.Orders, function (value, key) {
                    if (value.Date) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    }
                    if (value.PaymentApprovalDate) {
                        value.PaymentApprovalDate = new Date(parseInt(value.PaymentApprovalDate.substr(6)));
                    }
                    if (value.BuyingApprovalDate) {
                        value.BuyingApprovalDate = new Date(parseInt(value.BuyingApprovalDate.substr(6)));
                    }
                    if (value.ProofAcceptanceDate) {
                        value.ProofAcceptanceDate = new Date(parseInt(value.ProofAcceptanceDate.substr(6)));
                    }


                });
            });
        $scope.BndChange = function (id, Bnd) {
            $http({ method: 'POST', url: '/Orders/AssignBnd', data: { Bndid: Bnd, id: id } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#' + id).hide();
                }
            });

        };
        $scope.LoadFileData = function (files, id) {

            for (var file in files) {
                formData.append("file", files[file]);
            }
            var Order = id;
            formData.append("Order", angular.toJson(Order));

            $http.post("/Orders/SaveProof", formData, {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#' + id).hide();
                }
            });
        };





    }]);


    myApp.controller('SubBndsCtrl', ['$http', '$scope', '$filter', function ($http, $scope) {
        $http.get("/SubBnds/GetSubBnds")
            .then(function (response) {
                $scope.SubBnds = response.data[0];
                $scope.empid = response.data[1];
                $scope.Bnd = response.data[2];
                $scope.Centers = response.data[3];

                angular.forEach($scope.SubBnds, function (value, key) {
                    if (value.Date) {
                        value.Date = new Date(parseInt(value.Date.substr(6)));
                    }


                });

            });


    }]);

})();
