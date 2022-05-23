library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_logic_unsigned.ALL;
use IEEE.STD_LOGIC_arith.ALL;

entity alu is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Enable : in  STD_LOGIC;
			  direccion:in std_logic_vector(3 downto 0);
			  clk:in std_logic;
           WE : in  STD_LOGIC);
end  alu;

architecture Behavioral of  alu is

begin
process (clk) is
variable a:std_logic_vector(31 downto 0):=x"00000000";
variable b:std_logic_vector(31 downto 0):=x"00000000";
variable tempa:integer:=0;
variable tempb:integer:=0;

variable suma:std_logic_vector(32 downto 0):=x"00000000"&'0';
variable multi:std_logic_vector(63 downto 0):=x"0000000000000000";
begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
	    if direccion=x"0" then
			a:=datos;
		 end if;
		 if direccion=x"1" then
			b:=datos;
		 end if;
--		 tempa:=conv_integer(a);
--		 tempb:=conv_integer(b);
--		 if (b=0) then 
--			status:=status or x"00000002";
--		 else 
--		   status:=status and x"fffffffd";
--		 end if;
--		 for i in 31 downto 0 loop
--			if (tempa >= tempb *2**i) then
--			 entero(i):='1';
--			 tempa:=tempa-tempb*2**i;
--			else
--  			 entero(i):='0';
--			end if;
--		end loop;
--		residuo:=CONV_STD_LOGIC_VECTOR(tempa, 32);
		suma:=('0'&a)+('0'&b);
		 multi:=a*b;
	 else
	 --Opciones de la alu:
	 --Direccion 0, se encuentra el operando A
	 --Direccion 1, se encuentra el operando B
	 --Direccion 2, A + B
	 --Direccion 3, A * B Parte baja
	 --Direccion 4, A * B Parte alta
	 --Direccion 5, A-B
	 --Direccion 6, And
	 --Direccion 7, Or 
	 --Direccion 8, Xor
	 --Direccion 9, Nor
	 --Direccion 10, Xnor
	 --Direccion 11, Nand
	 --Direccion 12, Barrel Shifter Izquierda en A, B lugares
	 --Direccion 13, Barrel Shifter Derecha en A, B lugares
	 --Direccion 14, Not A
	 --Direccion 15, Status:
			--					en la posicion 0 esta el acarreo
			--					en la posicion 1 A > B?
			--					en la posicion 2 A == B?
			--					en la posicion 3 A == 0?
			--					en la posicion 4 B == 0?
	 
	    if direccion=x"0" then
			datos<=a;
		 end if;
	    if direccion=x"1" then
			datos<=b;
		 end if;
	    if direccion=x"2" then
			datos<=suma(31 downto 0);
		 end if;
	    if direccion=x"3" then

			datos<=multi(31 downto 0);
		 end if;
	    if direccion=x"4" then

			datos<=multi(63 downto 32);
		 end if;
	    if direccion=x"5" then
			datos<=a-b;
		 end if;
	    if direccion=x"6" then
			datos<=a and b;
		 end if;
		    if direccion=x"7" then
			datos<=a or b;
		 end if;
	    if direccion=x"8" then
			datos<=a xor b;
		 end if;
		    if direccion=x"9" then
			datos<=a nor b;
		 end if;
	    if direccion=x"a" then
			datos<=a xnor b;
		 end if;
		    if direccion=x"b" then
			datos<=a nand b;
		 end if;
	    if direccion=x"c" then
		  
for i in 31 downto 0 loop
		if (CONV_integer(b) = i) then
		   for c in 31 downto i loop
					datos(c)<=a(c-i);
			 end loop;
			 for c in i downto 0 loop
					datos(c)<='0';
			 end loop;
      end if;			 
end loop;
			
		 end if;
		 
		if direccion=x"d" then
for i in 31 downto 0 loop
		if (CONV_integer(b) = i) then
		   for c in 31-i downto 0 loop
					datos(c)<=a(c+i);
			 end loop;
			 for c in 31 downto 31-i loop
					datos(c)<='0';
			 end loop;
      end if;			 
end loop;			
		 end if;
	    if direccion=x"e" then
			datos<=not a;
		 end if;
		 
	    if direccion=x"f" then
		 datos(0)<=suma(32);
		 if a>b then
			datos(1)<='1';
		 else
		   datos(1)<='0';
		 end if;
		 if a=b then
			datos(2)<='1';
		 else
		   datos(2)<='0';
		 end if;
		 if a=x"00000000" then
			datos(3)<='1';
		 else
		   datos(3)<='0';
		 end if;
		 if b=x"00000000" then
			datos(4)<='1';
		 else
		   datos(4)<='0';
		 end if;
         datos(31 downto 5)<=(others=>'0');
		 end if;
	    
	    
	 end if;
 


else
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;
end process;

end Behavioral;
