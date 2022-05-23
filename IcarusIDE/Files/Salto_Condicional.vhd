library IEEE;
use IEEE.STD_LOGIC_1164.ALL;

entity SaltoCondicional is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Direccion: in std_logic_vector(2 downto 0);
			  Enable : in  STD_LOGIC;
			  clk:in std_logic;
           WE : in  STD_LOGIC);
end  SaltoCondicional;

architecture Behavioral of  SaltoCondicional is

begin
process (clk) is
variable a:std_logic_vector(31 downto 0):=x"00000000";
variable b:std_logic_vector(31 downto 0):=x"00000000";
variable nomayor,mayor:std_logic_vector(31 downto 0):=x"00000000";
variable noigual,igual:std_logic_vector(31 downto 0):=x"00000000";
begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
	   if direccion="000" then
	    a:=datos;
		end if;
	   if direccion="001" then
	    b:=datos;
		end if;
	   if direccion="010" then
	    mayor:=datos;
		end if;
	   if direccion="011" then
	    nomayor:=datos;
		end if;
	   if direccion="100" then
	    igual:=datos;
		end if;
	   if direccion="101" then
	    noigual:=datos;
		end if;
	 else
	 	if direccion="110" then
		if a>b then
		  datos<=mayor;
		 else
		 datos<=nomayor;
		end if;
		end if;
	 	if direccion="111" then
		if a=b then
		  datos<=igual;
		 else
		 datos<=noigual;
		end if;
		end if;

	 end if;
 


else
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;
end process;

end Behavioral;

