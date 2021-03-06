
Select * from ScoringSystems


  Insert into ScoringSystems
  (
      Id,
      ClubId,
      Name,
      DiscardPattern,
      ParentSystemId

  )
  VALUES
  (
   NEWID(),
   NULL,
   'High Point Percentage For Series',
   '0,1',
   	NULL
  )

Declare @SystemId UNIQUEIDENTIFIER
set @SystemId = (
  SELECT
   Id  
  From ScoringSystems where Name = 'High Point Percentage For Series')


  Insert into ScoreCodes
  (
      Id,
      Name,
      Description,
      PreserveResult,
      Discardable,
      Started,
      FormulaValue,
      AdjustOtherScores,
      CameToStart,
      Finished,
      Formula,
      ScoreLike,
      ScoringSystemId
  )
  VALUES
  (NewId(),'TIE','Tied result',1,1,1,NULL,0,1,1,'TIE', NULL, @SystemId),
  (NewId(),'SCP','Scoring Penalty rule 44.3',1,1,1,20,0,1,1,'PLC%',NULL, @SystemId),
  (NewId(),'DNS','Came to start area but did not start',0,1,0,0,null,1,0,'FIX',NULL,@SystemId),
  (NewId(),'RET','Retired',1,1,1,0,1,1,1,'FIX',NULL,@SystemId),
  (NewId(),'ZFP','20% Penalty under rule 30.2',1,1,1,20,0,1,1,'PLC%',NULL,@SystemId),
  (NewId(),'DPI','Discretionary penalty',1,1,1,20,0,1,1,'PLC%',NULL,@SystemId),
  (NewId(),'RDGAve','Redress: average of other races',1,1,1,NULL,0,1,1,'AVE',NULL,@SystemId),
  (NewId(),'UFD','Disqualification under rule 30.3',0,1,0,0,0,1,0,'FIX',NULL,@SystemId),  
  (NewId(),'BFD','Disqualification under rule 30.4',0,0,0,0,0,1,0,'FIX',NULL,@SystemId),
  (NewId(),'DNF','Started but did not finish',0,1,1,0,1,1,0,'FIX',NULL,@SystemId),
  (NewId(),'OCS','On course side at start or broke rule 30.1',0,1,0,0,1,1,0,'FIX',NULL,@SystemId),
  (NewId(),'DSQ','Disqualification',1,1,1,0,1,1,1,'FIX',NULL,@SystemId),
  (NewId(),'RDG','Redress: points set by protest hearing',1,1,1,NULL,0,1,1,'MAN',NULL,@SystemId),
  (NewId(),'DNE','Disqualification that is not excludable',0,0,1,0,1,1,1,'FIX',NULL,@SystemId),
  (NewId(),'DNC','Did not come to starting area',0,1,0,0,NULL,0,0,'FIX',NULL,@SystemId)


