   M S B A M L   `   `   `  ����H  DWpfMVVMSudoku, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:WpfMVVMSudokuWpfMVVMSudoku  5   	       WpfMVVMSudoku.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:WpfMVVMSudoku     5   	   -   5          � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      � 
������ � ColumnDefinitions 5
   
   �� 5      $��2* �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��5��6'   $��Stretch=�61   $��Center=�6M   �� 5   
   $��2��6   .+�5      �� -   5        $��Новая игра��5      $
��5,10q�6?   	� 5      -   5        $��rbEasy��5   6   $��5q�6    d� 	IsChecked$
 TrueY�6%   .��6D   Новичок5      	� 5      -   5        $��rbMaster��5   %   $��5q�6   .��66   Мастер5      	� 5      -   5        $��rbExpert��5   %   $��5q�6   .��65   Эксперт5      �� 5      $!��Сохранить игру��6   $
��5,10q�6N   ��6.   �� *SaveCommand+6N   �� 5      $!��Загрузить игру��6   $��5q�6N   ��6.   �� *LoadCommand+6N   �� 5      $��Проверить��6   $
��5,60q�6K   $��50��6Y   ��6)   �� *CheckCommand+6Y   5      � 5   
   -   5        $��grid1��5      $��0��6   .+�5      �� ��6   �� *Points+��5      �� 5      .R�5       �� -   5        ��5    !   �� *	Current+ �� UpdateSourceTrigger$ PropertyChanged=�
 � Tag 6b   �� $ PropertyChanged=�6�   5!      ��5#      �� 5$      .R�5%      A�  A� Rows$ 9��6&    A� Columns$ 9��6/   5'      5(      5*      5+      5,      5-      