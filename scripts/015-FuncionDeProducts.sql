CREATE FUNCTION dbo.ConvertDaysToFullNames (@abbreviatedDays VARCHAR(7))
RETURNS VARCHAR(40)
AS
BEGIN
    DECLARE @fullDays VARCHAR(40) = ''

    IF CHARINDEX('L', @abbreviatedDays) > 0 SET @fullDays += 'Lunes, '
    IF CHARINDEX('K', @abbreviatedDays) > 0 SET @fullDays += 'Martes, '
    IF CHARINDEX('M', @abbreviatedDays) > 0 SET @fullDays += 'Miércoles, '
    IF CHARINDEX('J', @abbreviatedDays) > 0 SET @fullDays += 'Jueves, '
    IF CHARINDEX('V', @abbreviatedDays) > 0 SET @fullDays += 'Viernes, '
    IF CHARINDEX('S', @abbreviatedDays) > 0 SET @fullDays += 'Sábado, '
    IF CHARINDEX('D', @abbreviatedDays) > 0 SET @fullDays += 'Domingo, '

    IF LEN(@fullDays) > 0
        SET @fullDays = LEFT(@fullDays, LEN(@fullDays) - 1)

    RETURN @fullDays
END
