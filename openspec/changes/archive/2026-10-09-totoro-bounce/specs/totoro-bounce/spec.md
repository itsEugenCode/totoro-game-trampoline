# Spec Delta

## Purpose

Даёт Тоторо роль живого батута: вход в зону белого живота запускает настраиваемый вертикальный импульс, заметно сильнее обычного прыжка, и не заменяет бег, воздух и камеру.

## ADDED Requirements

### Requirement: Belly trampoline zone
Белый живот Тоторо MUST быть bounce surface. Зона MUST совпадать с белым пятном живота и MUST NOT накрывать голову и лапы. Коллайдер зоны MUST быть trigger, чтобы игрок мог войти в неё коротким прыжком рядом с Тоторо.

#### Scenario: Enter belly zone with a short hop
- **WHEN** игрок подходит близко к Тоторо и коротким прыжком входит в зону белого живота
- **THEN** он оказывается в bounce surface и получает вертикальный импульс

### Requirement: Vertical impulse
Вход в bounce surface MUST давать игроку вертикальный импульс вверх, после которого он покидает зону.

#### Scenario: Bounce upward
- **WHEN** игрок входит в bounce surface живота
- **THEN** он получает скорость вверх и уходит из зоны

### Requirement: Inspector impulse
Сила вертикального импульса MUST задаваться в Inspector. Изменение этого значения MUST менять высоту отскока при следующем запуске Play без правки кода.

#### Scenario: Change bounce height from the Inspector
- **WHEN** в Inspector увеличена сила импульса и сцена запущена снова
- **THEN** игрок подлетает выше, чем до изменения

### Requirement: Air control after bounce
После импульса горизонтальное управление из `core-movement` MUST сохраняться в полёте. Импульс MUST NOT обнулять горизонтальную скорость. Отпускание горизонтальной команды в этом полёте MUST NOT останавливать игрока сразу.

#### Scenario: Steer after bounce
- **WHEN** игрок получил импульс, находится в полёте и держит горизонтальную команду
- **THEN** горизонтальная скорость меняется в эту сторону и не превышает максимальную скорость

#### Scenario: Keep momentum after bounce
- **WHEN** игрок получил импульс, летит в сторону и отпускает горизонтальную команду
- **THEN** он не останавливается мгновенно

### Requirement: Repeat bounce
Каждый новый вход в bounce surface после выхода из неё MUST давать новый вертикальный импульс. Пока контакт с зоной не прерван, следующий импульс MUST NOT выдаваться.

#### Scenario: Bounce again after leaving
- **WHEN** игрок после отскока снова входит в bounce surface
- **THEN** он получает новый вертикальный импульс

#### Scenario: No extra impulse while still in contact
- **WHEN** игрок уже получил импульс и ещё не покинул bounce surface
- **THEN** дополнительный импульс не выдаётся

### Requirement: Camera follows the high bounce
Камера MUST сопровождать игрока на высоком отскоке, в том числе по вертикали, с тем же настроенным следованием, которое уже держит в кадре бег и прыжок.

#### Scenario: High bounce stays in view
- **WHEN** игрок получает импульс и поднимается выше обычного прыжка
- **THEN** камера следует за ним по вертикали и не остаётся на высоте старта

### Requirement: Bounce stronger than the ground jump
Настроенный импульс MUST поднимать игрока заметно выше, чем обычный прыжок с земли при текущей силе этого прыжка.

#### Scenario: Apex above a ground jump
- **WHEN** игрок прыгает с земли, а затем входит в bounce surface
- **THEN** верхняя точка отскока заметно выше верхней точки обычного прыжка

### Requirement: Ground jump unchanged
Bounce MUST NOT подменять обычный прыжок с земли и MUST NOT давать прыжок по команде, пока игрок в воздухе после импульса.

#### Scenario: Jump from the ground still works
- **WHEN** игрок стоит на земле, не в bounce surface, и нажимает прыжок
- **THEN** выполняется один обычный прыжок с земли

#### Scenario: No command jump after bounce
- **WHEN** игрок уже получил импульс, находится в воздухе и нажимает прыжок
- **THEN** дополнительный прыжок по команде не выполняется
