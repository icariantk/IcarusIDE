library IEEE;
use IEEE.STD_LOGIC_1164.ALL;
use IEEE.STD_LOGIC_unsigned.ALL;

entity Delay is
    Port ( Datos : inout  STD_LOGIC_VECTOR (31 downto 0):="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
			  Direccion: in std_logic_vector(1 downto 0);
			  Enable : in  STD_LOGIC;
			  clk:in std_logic;
           WE : in  STD_LOGIC);
end  Delay;

architecture Behavioral of  Delay is

--00  A donde saltar si se cumple el delay
--01  A donde saltar si no se cumple el delay
--10  Delay
--11  Resultado
begin

process (clk) is

variable adonde,adondeno,delay:std_logic_vector(31 downto 0):=x"00000000";

begin

if clk'event and clk='1' then
if enable='1' then
    if we='1' then
	   if direccion="00" then
	    adonde:=datos;
		end if;
	   if direccion="01" then
	    adondeno:=datos;
		end if;
	   if direccion="10" then
	    delay:=datos;
		end if;	   
	 else
	 	if direccion="00" then
			datos<=adonde;
		end if;
		if direccion="01" then
			datos<=adondeno;
		end if;
		if direccion="10" then
		   datos<=delay;
		end if;
	 	if direccion="11" then
		   if delay=x"00000000" then
			  datos<=adonde;
			else
				datos<=adondeno;
			end if;
		end if;

	 end if;
 


else
if delay/=x"00000000" then
  delay:=delay-x"00000001";
end if;
  datos<="ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
end if;
end if;
end process;

end Behavioral;

