EXTERNAL getQuestStatus(questName)
VAR questStatus = ""
-> main

=== main ===
~getQuestStatus("questNamgung")
{questStatus == "unobtained":
What do you want, Jin? Still trying to prove yourself?
[He scoffs, clearly uninterested.]
    -> END
}
{questStatus == "success":
Hmph, you got lucky this time, little brother. But don’t think this makes you my equal.
[His voice is sharp, but there's a hint of grudging respect in his tone.]
    -> END
}
{questStatus == "fail":
Hah! Did you really think you could beat me? You’ll never catch up to me, Jin. Let alone our eldest brother!
[He smirks, the pride and arrogance thick in his words.]
    -> END
}