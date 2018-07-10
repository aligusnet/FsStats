module ``Sample Statistics tests``

open Xunit
open FsUnit.Xunit

open FsStats.SummaryStatistics

// Generated by Normal.Distribution(10.0, 2.0).Samples 30
let data = [|13.7858911526618; 9.55203558961586; 8.73590083796644; 14.6446895575045; 7.61758207758193; 
             7.81767470271187; 9.1694740424746; 11.6114177177951; 10.2582801800003; 8.540936510641; 
             7.98765220604253; 11.9092751627251; 6.04344060258961; 7.64167613512391; 10.5588747466957; 
             9.7008682321666; 11.3431376347234; 9.21564519814689; 9.911795373412; 10.1239694706768; 
             10.7603987140471; 10.4357884213866; 10.3055137320688; 12.2362093650794; 8.77565517588214; 
             6.43386624817808; 12.0007153989415; 12.8771963800697; 9.24429657175203; 10.3720301023293|]

[<Fact>]
let ``Summary Statistics`` () = 
    let stats = create data
    mean stats |> should (equalWithin 1e-5)  9.98706
    stddev stats |> should (equalWithin 1e-5) 2.02096
    stderr stats |> should (equalWithin 1e-5) 0.36898
    variance stats |> should (equalWithin 1e-5) 4.08428
    skewness stats |> should (equalWithin 1e-5) 0.23142
    kurtosis stats |> should (equalWithin 1e-5)  2.74207
    size stats |> should equal 30
    isNormalApproximationApplicable stats |> should equal true


[<Fact>]
let ``Temperature and ice cream sales should have strong positive correlation`` () =
    let temperature = create [|14.2; 16.4; 11.9; 15.2; 18.5; 22.1; 19.4; 25.1; 23.4; 18.1; 22.6; 17.2|]
    let iceCreamSales = create [|215.0; 325.0; 185.0; 332.0; 406.0; 522.0; 412.0; 614.0; 544.0; 421.0; 445.0; 408.0|]
    correlation temperature iceCreamSales |> should (equalWithin 1e-5) 0.95751


[<Fact>]
let ``Properties of skewness and kurtosis`` () = 
    let stats = create data
    kurtosis stats |> should greaterThanOrEqualTo ((skewness stats) ** 2.0 + 1.0)



[<Theory>]
[<InlineData("2.0, 0.0, 3.0, 1.0", 0.0, 0.5, 1.5, 2.5, 3.0)>]
[<InlineData("2.0, 0.0, 4.0, 3.0, 1.0", 0.0, 1.0, 2.0, 3.0, 4.0)>]
[<InlineData("2.0, 0.0", 0.0, 0.0, 1.0, 2.0, 2.0)>]
let ``Five-number summary`` (sampleString: string, q0, q1, q2, q3, q4) = 
    let sample = sampleString.Split(',') |> Array.map float
    let stats = create sample
    fivenum stats |> should equal (q0, q1, q2, q3, q4)


[<Theory>]
[<InlineData("2.0, 0.0, 3.0, 1.0", 0.5, 1.5)>]
[<InlineData("2.0, 0.0, 4.0, 3.0, 1.0", 0.5, 2.0)>]
[<InlineData("2.0, 0.0", 0.5, 1.0)>]
[<InlineData("2.0, 0.0, 4.0, 3.0, 1.0", 0.1, 0.0)>]
[<InlineData("2.0, 0.0, 4.0, 3.0, 1.0", 0.2, 0.5)>]
let ``Percentile`` (sampleString: string, k, expected) = 
    let sample = sampleString.Split(',') |> Array.map float
    let stats = createAndSort sample
    percentile stats k |> should equal expected


[<Theory>]
[<InlineData("0.0, 1.0", 0.5)>]
[<InlineData("0.0, 1.0, 2.0", 1.0)>]
let ``Meadian`` (sampleString: string, expected) =
    let sample = sampleString.Split(',') |> Array.map float |> Array.sort
    let stats = createWithSorted sample
    median stats |> should (equalWithin 1e-10) expected


[<Theory>]
[<InlineData("2.0, 0.0, 3.0, 1.0", 2.0)>]
[<InlineData("2.0, 0.0, 4.0, 3.0, 1.0", 2.0)>]
[<InlineData("2.0, 0.0", 2.0)>]
let ``Interquartile range`` (sampleString: string, expected) = 
    let sample = sampleString.Split(',') |> Array.map float
    let stats = create sample
    iqr stats |> should (equalWithin 1e-10) expected


[<Fact>]
let ``Margin of Error and Confidence Interval`` () =
    let stats = create data
    marginOfError stats 0.95 |> should (equalWithin 1e-5) 0.72284
    let l, r = confidenceInterval stats 0.95
    l |> should (equalWithin 1e-5) 9.26422
    r |> should (equalWithin 1e-5) 10.7099
