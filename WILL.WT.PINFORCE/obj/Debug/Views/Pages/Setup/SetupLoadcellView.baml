   M S B A M L   `   `   `  ����N  JWILL.WT.PINFORCE, Version=0.0.4347.1, Culture=neutral, PublicKeyToken=nullW0clr-namespace:WILL.WT.PINFORCE.Views.Pages.Setup"WILL.WT.PINFORCE.Views.Pages.Setup  5      a5clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Setup'WILL.WT.PINFORCE.ViewModels.Pages.Setup  5      3clr-namespace:WILL.WT.PINFORCEWILL.WT.PINFORCE  5      G CTS.FW.Wpf.v2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2TS.FW.Wpf.v2.Controls 5	      K GTS.FW.Wpf, Version=1.0.9053.17879, Culture=neutral, PublicKeyToken=nullJ3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.WpfTS.FW.Wpf.Controls 5
      :    4WILL.WT.PINFORCE.Views.Pages.Setup.SetupLoadcellView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Blocal0clr-namespace:WILL.WT.PINFORCE.Views.Pages.Setup     5      Dvm5clr-namespace:WILL.WT.PINFORCE.ViewModels.Pages.Setup     5      -emclr-namespace:WILL.WT.PINFORCE     5      Htc9clr-namespace:TS.FW.Wpf.v2.Controls;assembly=TS.FW.Wpf.v2     5	      Bts3clr-namespace:TS.FW.Wpf.Controls;assembly=TS.FW.Wpf     5
         xPageSub#��[  5        � 	Resources  5        
xConnector&   TS.FW.Wpf.v2.Controls.GroupBox3D � RowDefinitions(  "TS.FW.Wpf.v2.Controls.CaptionValue   Caption   SizeCaption$  TS.FW.Wpf.v2.Controls.ComboBtn  	xComboBtn!   WILL.WT.PINFORCE.EnumHelper   
SerialPort �� Source �� Text � ColumnDefinitions$  TS.FW.Wpf.v2.Controls.EventBtn  	xEventBtn  xStatus �� HeaderStringFormat$  TS.FW.Wpf.v2.Controls.OnOffBtn		   On

   Off J� IsHitTestVisible   	IsChecked'  !TS.FW.Wpf.v2.Controls.TextBlock3D  
xTextState   Text �� StringFormat��%�  &	       7  7  7  &	 t    7  �� 5   
   .R�5        $��1��6   $��1��6,   ��69   �� *Name+.��5      �  5      �� 5      �� 5      .+�5        $��0��6&   $ COM Port��63   $	 150Q�6F   $��2��6X   .��5        8��  ��6I   �� # Z  6�   �� *Port+5      � 5      $��1��6    5      �� 5      �� 5      .+�5"        $��0��6&   $
��Open��66   8�� 
��� � �$
��OPEN��6�   ��6{   �� *OnConnectionCmd+6�     5#      $��1��6&   $��Close��66   8�� 
���  ��$��CLOSE��6�   ��6z   �� *OnConnectionCmd+6�   5$      5%      5&      5'      �� 5(   
   .R�5)        $��1��6   $��1��6,   $ 
{0} STATUS��6Q   ��69   �� *Name+.��5+      �  5,      �� 5-      �� 5.      .+�52        $��0��6&   $ Communication��63   $	 150Q�6K   $��2��6]   .��53        $	 	Connected��6&   $
 Disconnected��65    .   6H   �� *IsOpen+6e   55        56      $��1��6&   $ 
Weight (g)��63   $	 150Q�6H   $��2��6Z   .��57        8��   6M   �� *Data+$ {0:f3} g��58      59      5:      5;      5<      .��5=      �  5>   
   �� 5?      $	��400 �6   �� 5@      $��1* �6   �� 5A      $��1* �6    5C   
   �� 5D      $	��150 �6   �� 5E      $	��150 �6   �� 5F      $��1* �6   .+�5J   
   �� $��0��6   $��0��6)   #��[ 66   ��6d   �� *LoadcellModel_1+�� 5M   
   $��0��6   $��1��6)   #��[ 66   ��6d   �� *LoadcellModel_2+�� 5P   
   $��1��6   $��0��6)   #��[ 66   ��6a   �� *LoadcellModel_1+�� 5S   
   $��1��6   $��1��6)   #��[ 66   ��6a   �� *LoadcellModel_2+5T      5V      