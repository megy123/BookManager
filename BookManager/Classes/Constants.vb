Module Constants

    Enum status
        None
        Reading
        Completed
        Dropped
        Hold
    End Enum

    Enum rating
        None
        Appalling
        Horrible
        VeryBad
        Bad
        Average
        Fine
        Good
        VeryGood
        Great
        Materpeace
    End Enum

    Public statusArr As String() = {"None",
                                    "Reading",
                                    "Completed",
                                    "Dropped",
                                    "Hold"}

    Public ratingArr As String() = {"None",
                              "(1)Appalling",
                              "(2)Horrible",
                              "(3)Very Bad",
                              "(4)Bad",
                              "(5)Average",
                              "(6)Fine",
                              "(7)Good",
                              "(8)Very Good",
                              "(9)Great",
                              "(10)Masterpeace"}

End Module
