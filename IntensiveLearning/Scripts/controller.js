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
    myApp.controller('AddMissionCtrl', ['$http', '$scope', function ($http, $scope) {
        if (!$scope.Is_Edit) {
        $http({ method: 'GET', url: '/Json/GetAllUsers' }).then(function successCallback(response) {
            $scope.Users = response.data;
            });
        }
        var x = false;
        $scope.ChangeManager = function (id) {
            $scope.Is_Edit = true;
            if (id) {
                $scope.ManagerId = id;
                $http({ method: 'GET', url: '/Json/GetAllUsers' }).then(function successCallback(response) {
                    $scope.Users = response.data;
                    angular.forEach($scope.Users, function (value, key) {
                        if (value.id == id) {
                            $scope.searchbox = value.name + " " + value.surname;;
                            $scope.HideSearch = true;
                        }
                    });
                });           
            }
        };
        $scope.TextChange = function () {
            $scope.HideSearch = false;
            if (x) {
                $scope.searchbox = "";
                x = false;
            }
        };
        $scope.ChangeManagerId = function (item) {
            $scope.ManagerId = item.id;
            $scope.searchbox = item.name + " " + item.surname;
            x = true;
            $scope.HideSearch = true;
        };
    }]);
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
        formData = new FormData();
        $scope.timeFunction = function (timeObj) {
            if (timeObj != null) {
                var min = timeObj.Minutes < 10 ? "0" + timeObj.Minutes : timeObj.Minutes;
                var hour = timeObj.Hours < 10 ? "0" + timeObj.Hours : timeObj.Hours;
                return hour + ':' + min;
            }
            return null;
        };
        $scope.RedirectToNewMission = function () {
            window.location.href = "/Missions/create"
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
        $scope.LoadFileData = function (files, id) {

            for (var file in files) {
                formData.append("file", files[file]);
            }
            formData.append("Mission", angular.toJson(id));

            $http.post("/json/MissionFilesSave", formData, {
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


    myApp.controller('StudentsCtrl', ['$http', '$scope', function ($http, $scope) {
        $scope.StudentNumberCol = true;
        $scope.SurnameCol = true;
        $scope.NameCol = true;
        $scope.FathersnameCol = true;
        $scope.MothersnameCol = true;
        $scope.BDateCol = true;
        $scope.CertificateCol = true;
        $scope.MarkCol = true;
        $scope.OldSchoolCol = true;
        $scope.StudentStateCol = true;
        $scope.SexCol = true;
        $scope.StateCol = true;
        $scope.SDateCol = true;
        $scope.EDateCol = true;
        $scope.CenterCol = true;
        $scope.RegimentCol = true;
        $scope.PeriodCol = true;
        $scope.StageCol = true;
        $scope.SignsCol = true;
        $scope.ProofCol = true;
        $scope.InfoCol = true;

        $scope.StudentNumberColCheckBox = true;
        $scope.SurnameColCheckBox = true;
        $scope.NameColCheckBox = true;
        $scope.FathersnameColCheckBox = true;
        $scope.MothersnameColCheckBox = true;
        $scope.BDateColCheckBox = true;
        $scope.CertificateColCheckBox = true;
        $scope.MarkColCheckBox = true;
        $scope.OldSchoolColCheckBox = true;
        $scope.StudentStateColCheckBox = true;
        $scope.SexColCheckBox = true;
        $scope.StateColCheckBox = true;
        $scope.SDateColCheckBox = true;
        $scope.EDateColCheckBox = true;
        $scope.CenterColCheckBox = true;
        $scope.RegimentColCheckBox = true;
        $scope.PeriodColCheckBox = true;
        $scope.StageColCheckBox = true;
        $scope.SignsColCheckBox = true;
        $scope.ProofColCheckBox = true;
        $scope.InfoColCheckBox = true;
        $scope.SearchContainer = function () {
            $('#SearchItemsBtn').toggle("fast");

        };
        $http({ method: 'GET', url: '/Json/SearchStudents' }).then(function successCallback(response) {
            $scope.Students = response.data[0];
            $scope.Centers = response.data[1];
            $scope.Cities = response.data[2];
            $scope.Regiments = response.data[3];
            $scope.Stages = response.data[4];
            $scope.Periods = response.data[5];
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


        $scope.StudentSearch = function () {
            $('#LoadingScreen').show();
            if ($scope.StudentSearchBoxData || $scope.StudentSearchBoxDate || $scope.CitiesChange || $scope.CentersChange || $scope.RegimentsChange || $scope.PeriodsChange || $scope.StagesChange) {
                var ToSendData = {
                    'SearchBoxData': $scope.StudentSearchBoxData,
                    'SearchBoxDate': $scope.StudentSearchBoxDate,
                    'CitiesChange': $scope.CitiesChange,
                    'CentersChange': $scope.CentersChange,
                    'RegimentsChange': $scope.RegimentsChange,
                    'PeriodsChange': $scope.PeriodsChange,
                    'StagesChange': $scope.StagesChange
                };
                $http({ method: 'POST', url: '/Json/SearchStudents', data: ToSendData }).then(function successCallback(response) {
                    $scope.Students = response.data;
                    $scope.HasSearched = true;
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
                    $scope.Students = response.data[0];
                    $scope.Centers = response.data[1];
                    $scope.Cities = response.data[2];
                    $scope.Regiments = response.data[3];
                    $scope.Stages = response.data[4];
                    $scope.Periods = response.data[5];
                    $scope.HasSearched = false;


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
        $scope.SearchContainer = function () {
            $('#SearchItemsBtn').toggle("fast");

        };
        $scope.NameCol = true;
        $scope.SurnameCol = true;
        $scope.BDateCol = true;
        $scope.CertificateCol = true;
        $scope.CTypeCol = true;
        $scope.OldJobCol = true;
        $scope.ExpYearsCol = true;
        $scope.InsideOrOutsideCol = true;
        $scope.StateCol = true;
        $scope.SDateCol = true;
        $scope.EDateCol = true;
        $scope.CenterCol = true;
        $scope.CityCol = true;
        $scope.EmployeeTypeCol = true;
        $scope.PeriodCol = true;
        $scope.SalaryCol = true;
        $scope.SignsCol = true;
        $scope.ProofCol = true;
        $scope.InfoCol = true;

        $scope.NameColCheckBox = true;
        $scope.SurnameColCheckBox = true;
        $scope.BDateColCheckBox = true;
        $scope.CertificateColCheckBox = true;
        $scope.CTypeColCheckBox = true;
        $scope.OldJobColCheckBox = true;
        $scope.ExpYearsColCheckBox = true;
        $scope.InsideOrOutsideColCheckBox = true;
        $scope.StateColCheckBox = true;
        $scope.SDateColCheckBox = true;
        $scope.EDateColCheckBox = true;
        $scope.CenterColCheckBox = true;
        $scope.CityColCheckBox = true;
        $scope.EmployeeTypeColCheckBox = true;
        $scope.PeriodColCheckBox = true;
        $scope.SalaryColCheckBox = true;
        $scope.SignsColCheckBox = true;
        $scope.ProofColCheckBox = true;
        $scope.InfoColCheckBox = true;




        $http({ method: 'GET', url: '/Json/Employees' }).then(function successCallback(response) {
            $scope.Exams = response.data[0];
            $scope.Centers = response.data[1];
            $scope.Cities = response.data[2];
            $scope.Periods = response.data[3];
            $scope.EmployeeTypes = response.data[4];


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

        $scope.EmployeeSearch = function () {
            $('#LoadingScreen').show();
            if ($scope.EmployeeSearchBoxData || $scope.EmployeeSearchBoxDate || $scope.CitiesChange || $scope.CentersChange || $scope.PeriodsChange || $scope.EmployeeTypesChange) {
                var ToSendData = {
                    'SearchBoxData': $scope.EmployeeSearchBoxData,
                    'SearchBoxDate': $scope.EmployeeSearchBoxDate,
                    'CitiesChange': $scope.CitiesChange,
                    'CentersChange': $scope.CentersChange,
                    'PeriodsChange': $scope.PeriodsChange,
                    'EmployeeTypesChange': $scope.EmployeeTypesChange
                };
                $http({ method: 'POST', url: '/Json/Employees', data: ToSendData }).then(function successCallback(response) {
                    $scope.Exams = response.data;
                    $scope.HasSearched = true;
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
                    $scope.Exams = response.data[0];
                    $scope.Centers = response.data[1];
                    $scope.Cities = response.data[2];
                    $scope.Periods = response.data[3];
                    $scope.EmployeeTypes = response.data[4];
                    $scope.HasSearched = false;
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
        $scope.timeFunction = function (timeObj) {
            if (timeObj != null) {
                var min = timeObj.Minutes < 10 ? "0" + timeObj.Minutes : timeObj.Minutes;
                var hour = timeObj.Hours < 10 ? "0" + timeObj.Hours : timeObj.Hours;
                return hour + ':' + min;
            }
            return null;
        };
        $scope.PaymentNum = function () {
            $('#LoadingScreen').show();

            $http({ method: 'POST', url: '/Orders/GetAccPayment', data: { Payment: $scope.Payment } }).then(function successCallback(response) {
                $scope.Orders = response.data[0];
                $scope.empid = response.data[1];
                $scope.Bnds = response.data[2];
                $scope.Payments = response.data[3];
                $scope.Orders.Date = new Date($scope.Orders.Date);
                $scope.ShowPrintBtn = true;
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
                $('#LoadingScreen').hide();

            });
        };
        $scope.RefuseOrderCommentFirstLevel = [];
        $scope.RefuseOrderCommentSecondLevel = [];
        $scope.RefuseOrderCommentThirdLevel = [];
        $scope.PaymentRefuseComment = [];
        $scope.BuyingRefuseComment = [];
        $scope.ProofRefuseComment = [];
        $scope.Bndid = [];
        $scope.Paymentid = [];
        $scope.TodaysDate = new Date();
        $scope.ShowPrintBtn = false;
        $http.get("/Orders/GetOrders")
            .then(function (response) {
                $scope.Orders = response.data[0];
                $scope.empid = response.data[1];
                $scope.Bnds = response.data[2];
                $scope.Payments = response.data[3];
                $scope.Orders.Date = new Date($scope.Orders.Date);
                $scope.Payment = "";
                $scope.ShowPrintBtn = false;
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
                $('#LoadingScreen').hide();

            });
        $scope.ConfirmOrderFirstLevel = function (Orderid) {
            $('#LoadingScreen').show();

            $http({ method: 'POST', url: '/Orders/ConfirmOrderFirstLevel', data: { id: Orderid } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.FirstLevelSign = true;
                        }
                    });
                };
            });
        };
        $scope.Quantity = function (Orderid) {
            $('#LoadingScreen').show();
            var Quantity;
            angular.forEach($scope.Orders, function (value, key) {
                if (value.id == Orderid) {
                    Quantity = value.Quantity;
                }
            });
            $http({ method: 'POST', url: '/Orders/Quantity', data: { id: Orderid, AcceptedQuantity: Quantity } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.QuantityChanged = true;
                        }
                    });
                };
            });

        };

        $scope.SendRefuseOrderFirstLevel = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/RefuseOrderFirstLevel', data: { id: Orderid, comment: $scope.RefuseOrderCommentFirstLevel[Orderid] } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.FirstLevelSign = false;
                            value.Comment = $scope.RefuseOrderCommentFirstLevel[Orderid];
                        }
                    });
                }
            });
        };
        $scope.AssignBnd = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/AssignBnd', data: { Bndid: $scope.Bndid[Orderid], id: Orderid } }).then(function successCallback(response) {
                if (response.data != null) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.Bnd = response.data;
                        }
                    });
                }
            });
        };
        $scope.ConfirmOrderSecondLevel = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/ConfirmOrderSecondLevel', data: { id: Orderid } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.SecondLevelSign = true;
                        }
                    });
                }
            });
        };
        $scope.SendRefuseOrderSecondLevel = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/RefuseOrderSecondLevel', data: { id: Orderid, comment: $scope.RefuseOrderCommentSecondLevel[Orderid] } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.SecondLevelSign = false;
                            value.Comment = $scope.RefuseOrderCommentSecondLevel[Orderid];
                        }
                    });
                }
            });
        };
        $scope.ConfirmOrderThirdLevel = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/ConfirmOrderThirdLevel', data: { id: Orderid } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.ThirdLevelSign = true;
                        }
                    });
                }
            });
        };
        $scope.SendRefuseOrderThirdLevel = function (Orderid) {
            $('#LoadingScreen').show();

            $http({ method: 'POST', url: '/Orders/RefuseOrderThirdLevel', data: { id: Orderid, comment: $scope.RefuseOrderCommentThirdLevel[Orderid] } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.ThirdLevelSign = false;
                            value.Comment = $scope.RefuseOrderCommentThirdLevel[Orderid];
                        }
                    });
                }
            });
        };

        $scope.CreatePayment = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/CreatePayment', data: { id: Orderid } }).then(function successCallback(response) {
                if (response.data != null) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.Paymentid = response.data;
                        }
                    });
                }
            });
        };

        $scope.ChoosePayment = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/ChoosePayment', data: { id: Orderid, Paymentid: $scope.Paymentid[Orderid] } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.Paymentid = Paymentid[Orderid];
                        }
                    });
                }
            });
        };
        $scope.PaymentApprove = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/PaymentApprove', data: { id: Orderid } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.PaymentApprove = true;
                            value.PaymentApprovalDate = new Date();
                        }
                    });
                }
            });
        };
        $scope.SendPaymentRefuse = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/PaymentRefuse', data: { id: Orderid, comment: $scope.PaymentRefuseComment[Orderid] } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.PaymentApprove = false;
                            value.Comment = $scope.PaymentRefuseComment[Orderid];
                        }
                    });
                }
            });
        };
        $scope.BuyingApprove = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/BuyingApprove', data: { id: Orderid } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.BuyingApprove = true;
                            value.BuyingApprovalDate = new Date();

                        }
                    });
                }
            });
        };
        $scope.SendBuyingRefuse = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/BuyingRefuse', data: { id: Orderid, comment: $scope.BuyingRefuseComment[Orderid] } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.BuyingApprove = false;
                            value.Comment = $scope.BuyingRefuseComment[Orderid];
                        }
                    });
                }
            });
        };
        $scope.ProofAcceptance = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/ProofAcceptance', data: { id: Orderid } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.ProofAcceptance = true;
                            value.ProofAcceptanceDate = new Date();
                        }
                    });
                }
            });
        };
        $scope.SendProofRefuse = function (Orderid) {
            $('#LoadingScreen').show();
            $http({ method: 'POST', url: '/Orders/ProofRefuse', data: { id: Orderid, comment: $scope.ProofRefuseComment[Orderid] } }).then(function successCallback(response) {
                if (response.data == true) {
                    $('#LoadingScreen').hide();
                    angular.forEach($scope.Orders, function (value, key) {
                        if (value.id == Orderid) {
                            value.ProofAcceptance = false;
                            value.Comment = $scope.ProofRefuseComment[Orderid];
                        }
                    });
                }
            });
        };



        $scope.ArrangeOrders = function (orderType) {
            $('#LoadingScreen').show();

            $http({ method: 'POST', url: '/Orders/GetOrdersArranged', data: { orderType: orderType } }).then(function successCallback(response) {
                $scope.Orders = response.data[0];
                $scope.empid = response.data[1];
                $scope.Bnds = response.data[2];
                $scope.Payments = response.data[3];
                $scope.ShowPrintBtn = false;
                $scope.Payment = "";
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
                $('#LoadingScreen').hide();

            });
        };
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
                if (response.status === 200) {
                    $scope.mission[id].Proof = response.data;
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
