   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullS.clr-namespace:WILL.WT.PINFORCE.Controls.Motion WILL.WT.PINFORCE.Controls.Motion  5      G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5      2    ,WILL.WT.PINFORCE.Controls.Motion.SvcAxisList   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      @local.clr-namespace:WILL.WT.PINFORCE.Controls.Motion     5      Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5         xDataGridBase#��[  5	   4   &   System.Windows.Controls.DataGrid    Columns  5
      4  .System.Windows.Controls.DataGridTemplateColumn  5   
   ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6!      Width5  /System.Windows.Controls.DataGridLengthConverter$
 1.5* 6/      CellTemplate 5      �� 5      .R�5      $  TS.FW.Wpf.v2.Controls.EventBtn    	xEventBtn#��[ 6"   ��5   "   �� *Name+��5   "   �� *	IsSeleted+  	xColorCvt �� 	Converter# [  �� ConverterParameter$
 Menu����5   "   ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType2   ,WILL.WT.PINFORCE.Controls.Motion.SvcAxisList"  	 �� Path$!	 DataContext.OnSelectAxisCmd���5   "   �� 5      5        5   
   $ Servo��6!   $ 60 60    5      �� 5      .R�5        #��[ 6"   $��Servo��5   @   ��5   "   �� *State.IsServoOn+  
xStringCvt# [ $ On/Off����5   "   �� *State.IsServoOn+# [ $ OnOff����5   "   �� *	OnAxisCmd+6@   5      5        5    
   $ Alarm��6!   $ 60 60    5!      �� 5"      .R�5#        #��[ 6"   $��Alarm��5&   @   ��5$   "   �� *State.IsAlarm+# [ $ 
Error/None����5%   "   �� *State.IsAlarm+# [ $ Alarm����5&   "   �� *	OnAxisCmd+6@   5'      5)      0  *System.Windows.Controls.DataGridTextColumn  5*   
   $
 Busy��6   $ 60 6+   
   
IsReadOnly
 .   GridTextBase1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [ 6H      Binding 6u   �� *State.IsBusy+# [ $ On/Off��  5+   
   $ Act Pos (mm)��6   $ * 63   
 . # [ 6O    6|   �� *State.ActPos+ �� StringFormat$ {0:f3}��  5,   
   $ Cmd Pos (mm)��6   $ * 63   
 . # [ 6O    6|   �� *State.CmdPos+$ {0:f3}��  5-   
   $  ��6!   $ 60 6+    5.      �� 5/      .R�50        #��[ 6"   $��Reset��51   "   $��Reset��52   @   ��6"   �� *	OnAxisCmd+6@   53      55      57      