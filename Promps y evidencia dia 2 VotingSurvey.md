***Promps:***



1. Aqui tengo una observacion, no me gusta que accedas a los contextos como \_context.Set<Voting>(), mejor usa directamente las entidades contexto ya configuradas en el DataBaseContext, asi: \_context.Votings, corrigelo
2. listo, ahora en adelante usaremos esta forma, Ahora vamos a continuar creando las diferentes entidades de repositorio en la altura de Application/Repositories donde vas agregar las interfaces con los metodos necesarios para cumplir con los casos de uso que tu mismo identificaste, por ahora, tambien ten encuenta que metodos tienen como responsable a una entidad en especifico, la idea es que se una interfaz sea referente a una entidad de dominio, asi como hicimos con IVoting que hace referencia a voting y asi en el repositorio de infrastructure en VotingRepositorie solo hace referencia al contexto de Votings. Asi tambien quiero que sean los demas.
3. listo, ahora si vamos a ir de nuevo a los casos de uso. Aplicando buenos principios de codigo limpio, si es necesario, creando clases privadas en los handlers, vamos a iniciar en la creacion de una votacion, como te comente antes, estoy manejando un patron de diseño CQRS, la carpeta UseCases creo una carpeta por cada entidad, ahi se manejaran los casos de uso segun la entidad de dominio que le corresponda, por ende, como se esta trabajando con el patron de diseño CQRS, en commands iria el CreateVoting, ahi dentro esta la carpeta de los handlers los cuales irian los archivos correspondientes a la clase recortada, en este caso es CreateVotingHandler, y hay otra carpeta para el manejo de las validaciones con fluentValidation, siguiendo este orden de ideas necesto que me crees los casos de uso necesarios los cuales son los que ya identificaste anteriormente y ya sabes que vas biendo que caso de uso le corresponde a que entidad, y cada entidad se manejaria con el patron de diseño CQRS. Se me paso decirte que si necesitas Dtos tambien en la altura de los CQRS puedes agregar en una carpeta llamada Dtos (en los casos de uso de voting ya esta el ejemplo de como seria).
4. Antes de continuar quisiera que nos detengamos un momento en la creacion de la votacion, por ejemplo yo no veo un metodo donde creemos los "Vote" de si y no, dado que debemos de crearlos ya que hacen parte de la tabla Vote donde estan las opciones de respuesta de la tabla Voting, o me equivoco?
5. esta bien entiendo ahora si, No quiero que persista opciones como catalogo como tu me indicas, mejor sigamos con el diseño que me sujieres. Ahora si continuemos, quiero que continuemos con el siguiente paso: "Siguiente paso: casos de uso adicionales por entidad (Voting: AddRecipients, EditBeforeStart, ExtendEnd; Vote: Select y Confirm; Queries para listados y detalles). ¿Procedo a generarlos con sus DTOs y handlers?"
6. si, estoy de acuerdo.
7. si, procede con ello.
8. Antes de continuar estoy deacuerdo con todos los archivos que creaste, excepto, con el nuevo UnitOfWork, deja el anterior como estaba y usa ese con los nombres que tiene, es que no veo necesario que llames los metodos con Async al final sabiendo que toda la aplicacion es asincrona, entonces no tiene sentido, corrige ello primero antes de que sigamos con los demas pasos.
9. si, sigamos, pero recuerda, tambien debes de agregar los archivos de validaciones con el fluentvalidation van en la carpeta validations en la altura de los handlers. veo que en los anteriores casos de uso no agregaste este archivo, valida en cual de los casos de uso se requiere y los agregas.
10. si, Continuamos con las queries de listado por creador y por destinatario.
11. si, Deseo agregar filtros por estado o paginación, pero ten presente que si vamosa agregar paginacion y queryparams debemos de usar los que ya tengo creados y segundo en las clases recortadas, debes de retornar entonces ApiResponse<PaginatorResponse<object>> para que tengamos consistencia el uso de paginador y los queryparams. Se que no sobra decir pero las clases recortadas donde necesiten paginacion deben de recibirla.
12. cuando me referia a que debian retornar  ApiResponse<PaginatorResponse<object>> ese objecto se refiere al Dto correspondiente que antes habias configurado para ese caso de uso, y aparte no inyectes los parametros por metodo primario si no como parametros de la clase para hacer mejor manejo de inicializacion, esto aplica para todos los queries y commands que hay en un caso de uso. otro aporte es que no es necesario que las clases recortadas sean clases celladas, quitales el sealed, no es necesario, ya que debemos de llamarlas en los controladores, o  si lo consideras necesario, dejalo.
13. ahora, antes de continuar, vamos a ir implementando lo que te dije de recibir en las clases recortadas los parametros como atributos estando dentro de la clase y no por metodo primario, en la clase recortada ListByCreator he dejado la forma como deseo realizar recibir los parametros, primeero para mejor legibilidad del codigo y segundo para la inicializacion correcta por si es necesario, esto quiero que lo apliques en todos las clases recortadas.
14. Ahora te pregunto, falta algun caso de uso por configurar ? segun los casos de uso que identificaste cual mas faltaria por configurar?
15. Antes de continuar, quiero hacer otra correccion, siguiendo los principios de responsabilidad de arquitectura limpia veo que en los queries de los casos de uso donde recibimos QueryParams veo que aplicamos el search y orderby en el handler, esto se deberia de aplicar en los metodos del repositorio, en capa de infrastructure. agregale el queryparam a los metodos del contrarto y tambien al repositorio. Donde refleje estos errores son en los archivos que te referencie pero si hay mas tambien corrigelos.
16. Ahora si podemos continuar, quisiera que continuemos con los siguientes casos de uso los cuales son: "Queries: •	ListVotingParticipants (lista de usuarios que votaron YES y NO con confirmación). •	GetVotingParticipantsSeparated (dos listas sí/no). •	GetUserVoteStatus (saber si el usuario ya votó, pendiente o confirmado). •	ListUpcoming (votaciones futuras del residente). •	ListActive (votaciones en curso filtradas). •	ListClosed (histórico). •	AdminGetVotingRecipients (listado de destinatarios asignados). •	AdminDashboardSummary (conteos: upcoming/active/closed)." y "Commands (Voting): •	CloseVotingEarly (cierre anticipado) para que el admin pueda finalizar antes de EndAt."
17. por ahora no, pero te pregunto, segun los casos de uso que identificamos de la HU, falta algun caso de uso por crear ?
18. Bueno, ahora como veo que todo va en orden vamos a realizar los controladores, ya te deje la base lista para aplicar los metodos http correspondientes segun los casos de uso, tienes que tener en cuenta varios aspectos: 1 - cada caso de uso es un endpoint . 2- los casos de uso que necesiten rol y userId se envian siempre por headers, por lo que cada metodo puede recibirlos con \[FormHeader (Name="")] T value. Se resalta que los unicos dos valores que se envian por headers son "Role" de tipo string y "UserId" de tipo Guid. 3 - en los enpoints deben de recibir la clase recortada segun sea el caso de uso y si necesitan alguno de los headers debes de asignarlos a la variable correspondiente, es decir, ejemplo:  Create(CreateVoting request, \[FormHeader (Name="UserId")] Guid userId), asi se llama el metodo y para asignar el valor de la clase recordata seria request = request with {CreatedById = userId}. 4 - retorna el match con valores que recibe, success => Ok(success), error => Problem(\[...error!])). Son dos controladores dado que en los casos de uso solo esta Vote y Voting.
19. Como cada metodo mediador retorna un ApiResponse, no es necesario el IMediator por lo que MatchResponse(result); no es necesario, cambialo por: "var result = await Sender.Send(request); return result.Match(success => Ok(result), error => Problem(\[.. error!]));" en todos los controladores.
20. Ahora estoy realizando una documentacion del aplicativo pero me estan pidiendo una descripcion como tal de cada caso de uso configurado, podrias generar esa descripcion para cada caso de uso ?.
21. ahora la lógica de negocio (reglas principales, validaciones clave, invariantes).
22. ahora La lógica de aplicación (cómo se orquestan los casos de uso y se interactúa con infraestructura).
23. ahora La lógica de aplicación (cómo se orquestan los casos de uso y se interactúa con infraestructura).



**NOTA:** He podido evidenciar que GitHub Copilot no guarda del todo las prompts o el historial de prompts no lo guarda bien dado que he hecho muchísimos mas y mas específicos con respecto a todo el proyecto elaborado. Estos son los que ha guardado.

***Descripción de casos de uso:***

**VOTING - COMMANDS**



CreateVoting Descripción: Permite al administrador crear una nueva votación Sí/No publicada inmediatamente. Actor: ADMIN. Entradas: Title, Description (pregunta), Question (debe ser YES\_NO), CreatedById, Options (debe contener YES y NO), StartDate, EndDate. Reglas: Solo ADMIN; StartDate < EndDate; título y descripción dentro de longitudes; una sola ventana activa; opciones fijas YES/NO. Salida: ApiResponse<Guid> con Id de la votación. Errores comunes: Rol inválido, rangos de fechas inválidos, opciones incorrectas.



AddRecipients Descripción: Asigna destinatarios (usuarios) a una votación ya creada. Actor: ADMIN. Entradas: VotingId, RecipientIds (lista de Guid). Reglas: Debe existir la votación; al menos un destinatario; evitar duplicados. Salida: ApiResponse<bool>. Errores: Voting inexistente, lista vacía.



EditVotingBeforeStart Descripción: Edita completamente título, descripción y ventana antes de que inicie. Actor: ADMIN. Entradas: VotingId, Title, Description, StartDate, EndDate. Reglas: Solo permitido si la votación no ha iniciado; StartDate < EndDate. Salida: ApiResponse<bool>. Errores: Intento de edición luego del inicio, fechas inválidas.



ExtendVotingEnd Descripción: Extiende (solo aumenta) la fecha de finalización después de iniciado y antes de cerrar. Actor: ADMIN. Entradas: VotingId, NewEnd. Reglas: NewEnd > EndAt actual; la votación debe estar activa (iniciada y no cerrada). Salida: ApiResponse<bool>. Errores: Nueva fecha menor o igual a la existente, votación cerrada/no iniciada.



CloseVotingEarly Descripción: Cierra anticipadamente la votación antes del EndDate original. Actor: ADMIN. Entradas: VotingId, AdminId. Reglas: Solo ADMIN; si ya está cerrada no hace nada más. Salida: ApiResponse<bool>. Errores: Rol inválido, voting inexistente.



**VOTE - COMMANDS**



SelectVote Descripción: Registra la selección (YES/NO) inicial del residente antes de confirmación. Actor: RESIDENT (destinatario). Entradas: VotingId, UserId, Option (VoteOption.Yes/No). Reglas: Usuario debe ser destinatario; ventana activa; no debe haber votado antes. Salida: ApiResponse<bool>. Errores: Fuera de ventana, usuario no destinatario, voto duplicado.



ConfirmVote Descripción: Confirma el voto previamente seleccionado, haciéndolo definitivo. Actor: RESIDENT. Entradas: VotingId, UserId. Reglas: Debe existir selección previa; no modificar opción ya confirmada. Salida: ApiResponse<bool>. Errores: Voto no encontrado, ya confirmado.



**VOTING - QUERIES**



GetVotingDetail Descripción: Obtiene detalle completo de una votación con totales y el voto del usuario (si aplica). Entradas: VotingId, UserId (opcional). Salida: ApiResponse<VotingDetailDto> (Title, Description, StartAt, EndAt, State, YesCount, NoCount, YourVote). Estado derivado: Upcoming / Active / Closed. Errores: Voting no encontrada.



ListByCreator Descripción: Lista paginada de votaciones creadas por un administrador. Entradas: CreatorId, Query (Search, OrderBy, PageNumber, PageSize). Salida: ApiResponse<PaginationResponse<VotingListItemDto>>. Orden soportado: title, startAt, endAt. Errores: Ninguno puntual (vacío si no hay datos).



ListForRecipient Descripción: Lista paginada de votaciones (de cualquier estado) donde el usuario es destinatario. Entradas: UserId, Query. Salida: ApiResponse<PaginationResponse<VotingListItemDto>>. Errores: Ninguno puntual.



ListVotingParticipants Descripción: Lista combinada de usuarios que votaron YES o NO (confirmados). Entradas: VotingId. Salida: ApiResponse<IReadOnlyList<Guid>> (IDs únicos). Errores: Voting no encontrada (interno en repo si aplica).



GetVotingParticipantsSeparated Descripción: Devuelve dos listas separadas (yes, no) de usuarios con votos confirmados. Entradas: VotingId. Salida: ApiResponse<(IReadOnlyList<Guid> yes, IReadOnlyList<Guid> no)>. Errores: Voting no encontrada.



AdminGetVotingRecipients Descripción: Lista de destinatarios asignados a una votación. Actor: ADMIN. Entradas: VotingId, AdminId. Salida: ApiResponse<IReadOnlyList<Guid>>. Errores: Rol inválido, voting inexistente.



AdminDashboardSummary Descripción: Muestra conteos globales (upcoming, active, closed) de las votaciones creadas por el admin. Entradas: AdminId. Salida: ApiResponse<(int upcoming, int active, int closed)>. Errores: Rol inválido.



GetUserVoteStatus Descripción: Indica estado del voto del usuario en una votación. Entradas: VotingId, UserId. Salida: ApiResponse<string?> con:

•	YES\_PENDING / NO\_PENDING

•	YES\_CONFIRMED / NO\_CONFIRMED

•	null si no ha votado. Errores: Voting no encontrada.



**ESTADOS Y REGLAS CLAVE REFERENCIADAS**



•	Upcoming: now < StartAt.

•	Active: StartAt <= now <= EndAt.

•	Closed: now > EndAt o cierre anticipado.

•	Un solo voto por usuario por votación.

•	Confirmación irreversible.

•	Ediciones completas solo antes de iniciar.

•	Extensión de fin solo hacia adelante.

•	Destinatarios requeridos para visibilidad y voto.



**ERRORES TRANSVERSALES**



•	Voting not found.

•	Only ADMIN ... (autorización).

•	User not recipient.

•	User already voted.

•	Invalid dates (Start/End).

•	Options must be YES/NO.

•	At least one recipient required.







***Lógica de negocio (reglas principales, validaciones clave, invariantes)***



**Dominio e invariantes por entidad**



•	User

•	Email válido (formato + longitud).

•	Nombres no vacíos y longitud ≤ 100.

•	Roles en mayúsculas. Un usuario puede tener múltiples roles únicos.

•	Solo usuarios activos considerados para destinatarios/votos.

•	Voting

•	Title obligatorio, longitud ≤ 200.

•	QuestionDescription obligatoria, longitud ≤ 1000.

•	QuestionType fijo: YES\_NO (representado como literal y no se persisten opciones).

•	Window: StartAt < EndAt (invariante temporal).

•	Publicación implícita en creación (IsPublished = true).

•	No se puede editar título, descripción ni fecha de inicio una vez iniciada.

•	Extensión solo permite aumentar EndAt (EndAt\_new > EndAt\_actual).

•	Cierre anticipado solo antes de EndAt y cambia estado a Closed.

•	Cada destinatario único (HashSet).

•	VotingRecipient

•	(VotingId, UserId) único.

•	Se requiere al menos un destinatario para visibilidad hacia el residente (regla de UI/negocio).

•	Vote

•	Una sola selección por (VotingId, UserId).

•	Estado: Selected (no confirmado) → Confirmed (irreversible).

•	Opción siempre Yes / No (enum VoteOption).

•	No se puede cambiar opción tras confirmación.

•	No se crea Vote sin que el usuario sea destinatario.

•	Confirmación requiere voto previo.



Reglas de autorización



•	ADMIN:

•	Crear votación.

•	Editar antes del inicio.

•	Extender fecha de cierre después del inicio.

•	Cierre anticipado.

•	Ver destinatarios.

•	Dashboard resumen.

•	RESIDENT:

•	Listar votaciones donde es destinatario (cualquier estado).

•	Seleccionar voto en estado Active.

•	Confirmar voto en estado Active.

•	Ver estado de su propio voto.

•	Ambos:

•	Obtener detalle de una votación (si destinatario; el admin creador la ve).



Estados y transiciones (Voting)



•	Upcoming: now < StartAt.

•	Active: StartAt ≤ now ≤ EndAt.

•	Closed: now > EndAt o cierre anticipado (CloseEarly). Transiciones válidas:

•	Upcoming → Active (llega StartAt).

•	Active → Closed (llega EndAt o cierre anticipado).

•	Upcoming → Closed (cierre anticipado antes de iniciar). Operaciones permitidas por estado:

•	Edit (completa): solo Upcoming.

•	ExtendEnd: únicamente Active.

•	CloseEarly: Upcoming o Active (no hace nada si ya Closed).

•	Vote select/confirm: solo Active.

•	Tally: cualquier estado (los votos confirmados cuentan).



Validaciones (FluentValidation / dominio)



•	CreateVoting:

•	Title, Description no vacíos.

•	Question == "YES\_NO".

•	StartDate < EndDate.

•	Options contiene EXACTAMENTE \["YES","NO"].

•	EditVotingBeforeStart:

•	VotingId válido.

•	Nuevos StartDate < EndDate.

•	ExtendVotingEnd:

•	NewEnd > EndDate actual.

•	AddRecipients:

•	Lista no vacía.

•	CloseVotingEarly:

•	VotingId y AdminId no vacíos.

•	SelectVote:

•	VotingId, UserId no vacíos.

•	Option ∈ {Yes, No}.

•	ConfirmVote:

•	VotingId, UserId no vacíos.



Precondiciones por comando



•	CreateVoting: Usuario con rol ADMIN.

•	AddRecipients: Voting existente + ADMIN.

•	EditVotingBeforeStart: Voting en Upcoming + ADMIN.

•	ExtendVotingEnd: Voting en Active + ADMIN.

•	CloseVotingEarly: Voting no cerrada + ADMIN.

•	SelectVote: Usuario destinatario + Voting Active + sin voto previo.

•	ConfirmVote: Voto existente no confirmado + Voting Active.

•	GetUserVoteStatus: Voting existente.



Invariantes temporales



•	StartAt y EndAt no se superponen con lógica adicional (no hay solapamiento prohibido entre votaciones).

•	Extender fin nunca retrocede EndAt.

•	Cierre anticipado fija EndAt = now (implícito por Window reconstruida).



Cálculo de resultados



•	TallyConfirmed: conteo solo de votos Confirmed (Yes / No).

•	Estado de voto usuario:

•	Null → sin participación.

•	YES\_PENDING / NO\_PENDING → voto seleccionado sin confirmar.

•	YES\_CONFIRMED / NO\_CONFIRMED → voto confirmado.



Persistencia y consistencia



•	Claves:

•	Voting: GUID.

•	Vote: GUID + constraint única (VotingId, UserId).

•	VotingRecipient: PK compuesta (VotingId, UserId).

•	Integridad referencial: FK hacia User y Voting en Vote/VotingRecipient.

•	Se evita duplicado de destinatario y voto vía estructuras internas + constraints DB.



Edge cases y mitigación



•	Intento de votar antes de inicio → rechazado.

•	Intento de votar luego del cierre → rechazado.

•	Extensión con fecha igual/más corta → error.

•	Reconexión: voto sin confirmar permanece (estado pending al consultar).

•	Cierre anticipado: no auto-confirma votos pendientes (podría añadirse servicio si se requiere).

•	Usuario sin rol correcto intentando acciones de admin → error autorizado.



Errores estándar



•	"Voting not found"

•	"Only ADMIN can ..."

•	"User not a recipient"

•	"User already voted"

•	"Voting not started yet"

•	"Voting already closed"

•	"Cannot edit after start except end date"

•	"New end must be later than current end"

•	"Vote not found"



Consideraciones de concurrencia



•	Condición de carrera en SelectVote / ConfirmVote mitigada por constraint única y confirmación irreversible.

•	Extender fin y cerrar anticipadamente: aplicar transacción para evitar estado inconsistente.



Resumen de prohibiciones



•	Cambiar opción luego de confirmación.

•	Editar ventana completa tras inicio.

•	Retroceder fecha de cierre.

•	Crear votación sin rol ADMIN.

•	Votar fuera de ventana.





***Lógica de aplicación (orquestación y comunicación con infraestructura)***



1\.	Flujo general



•	Los controladores reciben la petición HTTP.

•	Extraen encabezados (UserId, Role) y parámetros (body, query).

•	Construyen el record (Command o Query) y lo envían vía MediatR (Sender.Send).

•	El handler ejecuta validaciones adicionales (rol, estado) y coordina repositorios + UnitOfWork.

•	Devuelve un ApiResponse<T> que el controlador transforma a Ok(...) o Problem(...).



2\.	Pipeline (MediatR + ValidationBehavior)



•	Cada request pasa por ValidationBehavior: si hay excepción se encapsula como ApiResponse.Failure.

•	FluentValidation (clases en carpeta Validations) asegura reglas sintácticas antes de la lógica de negocio.

•	Los handlers contienen solo lógica de orquestación y autorización (no validaciones de formato básico).



3\.	Commands (Write side)



•	Handlers de creación/edición/acciones mutadoras:

•	Validan rol usando IUser.HasRoleAsync cuando aplica (ADMIN).

•	Recuperan y/o construyen el agregado (Voting) a través de repositorio IVoting.

•	Ejecutan métodos del dominio (ej: EditBeforeStart, ExtendEnd, CloseEarly).

•	Agrupan operaciones en una transacción explícita: BeginTransaction → repo ops → SaveChanges → CommitTransaction.

•	En fallo: RollbackTransaction y retorno ApiResponse.Failure.

•	No exponen lógica del ORM ni DbContext directamente; todo pasa por repositorios.



4\.	Queries (Read side)



•	Recuperan colecciones o entidades vía métodos especializados del repositorio.

•	Proyección a DTOs (ej: VotingListItemDto, VotingDetailDto) en el handler (sin lógica de persistencia).

•	Paginación: repositorio aplica filtros (Search/Order) y el handler solo paginate (PaginationResponse.Create).

•	Cálculo de estado (Upcoming/Active/Closed) derivado de ventana temporal (VotingWindow).



5\.	Repositorios (Infraestructura)



•	Encapsulan EF Core y expresiones LINQ (filtros, orden, búsqueda).

•	IVoting define operaciones de alto nivel (listar por estado, agregar destinatarios, cerrar, extender).

•	Separación clara: el dominio no conoce EF; el repositorio traduce a consultas sobre DbSet<T>.

•	Métodos de lectura retornan colecciones listas para proyectar; métodos de escritura solo preparan entidades (Commit fuera).



6\.	UnitOfWork



•	Gestiona la transacción y confirmación atómica de cambios.

•	Los handlers son los límites transaccionales: cada command inicia y finaliza su propia transacción explícita.

•	Evita múltiples commits parciales por operación; reduce riesgo de inconsistencias.



7\.	Controladores



•	Minimalistas: ensamblan el request (records mutados con with { ... } para headers).

•	No contienen lógica de negocio ni acceso a datos.

•	Uso consistente de ApiResponse mediante Match(...).



8\.	Middleware (ApiMiddleware)



•	Obtiene X-Role y X-User-Id y los coloca en HttpContext.Items.

•	Controladores piden el header directamente para mayor claridad.

•	Validación fina de rol se realiza en handlers (punto único de autorización de negocio).



9\.	Manejo de errores



•	Excepciones en dominio o repositorio capturadas por handlers (try/catch) y devueltas como ApiResponse.Failure.

•	Mensajes de error estándar permiten tratamiento uniforme en front.

•	Validaciones de formato y reglas simples se frenan antes de la lógica (FluentValidation).



10\.	Límites transaccionales



•	Solo commands abren transacciones.

•	Queries son lecturas sin transacción explícita (idempotentes).

•	No se mezclan writes y reads dentro de la misma operación para mantener claridad (CQRS).



11\.	Responsabilidades por capa



•	Presentación: HTTP → Request → MediatR.

•	Aplicación: Orquestación, autorización, transacciones, ensamblado de DTOs.

•	Dominio: Invariantes y reglas sobre agregados (clases Voting, Vote).

•	Infraestructura: Persistencia EF Core, materialización de entidades, filtros y ordenamientos.



12\.	Estados y sincronización



•	Estado de votación derivado al vuelo (no almacenado redundante).

•	Estado de voto (pending vs confirmed) controlado por dominio; confirmación irreversible.

•	No hay eventos de dominio todavía; se pueden agregar luego para side effects (notificaciones, auditoría).



13\.	Extensibilidad prevista



•	Fácil añadir nuevos handlers (ej. Exportar resultados) reutilizando repositorios.

•	Posible reemplazo de EF por otro almacenamiento sin tocar lógica de orquestación.

•	Incorporación futura de domain events añadiendo publicación en handlers post-commit.



14\.	Consistencia y concurrencia



•	Restricciones únicas (VotingId/UserId) protegen contra doble voto.

•	Transacciones cortas y directas minimizan conflictos.

•	Validaciones de estado antes de ejecutar métodos del agregado.





***Validaciones implementadas***



Nivel de entrada (Application layer con FluentValidation)



•	CreateVotingValidation

•	Title: requerido, ≤ 200 caracteres.

•	Description: requerida, ≤ 1000.

•	Question: debe ser exactamente "YES\_NO".

•	CreatedById: requerido.

•	StartDate / EndDate: ambos requeridos, EndDate > StartDate.

•	Options: lista requerida con exactamente 2 elementos: "YES" y "NO".

•	AddRecipientsValidation

•	VotingId: requerido.

•	RecipientIds: no nulo y al menos un destinatario.

•	EditVotingBeforeStartValidation

•	VotingId: requerido.

•	Title: requerido, ≤ 200.

•	Description: requerida, ≤ 1000.

•	StartDate / EndDate: EndDate > StartDate.

•	ExtendVotingEndValidation

•	VotingId: requerido.

•	NewEnd: requerido.

•	CloseVotingEarlyValidation

•	VotingId: requerido.

•	AdminId: requerido.

•	SelectVoteValidation

•	VotingId: requerido.

•	UserId: requerido.

•	Option: enum válido (Yes/No).

•	ConfirmVoteValidation

•	VotingId: requerido.

•	UserId: requerido.



Nivel de dominio (invariantes dentro de entidades y value objects)



•	VotingWindow

•	StartsAt != default.

•	EndsAt != default.

•	StartsAt < EndsAt.

•	Voting

•	Title no vacío, longitud ≤ 200.

•	QuestionDescription no vacía, longitud ≤ 1000.

•	Edición completa solo antes de inicio (EditBeforeStart lanza excepción si ya inició).

•	ExtendEnd: solo si ya inició y no terminó; NewEnd > StartAt y NewEnd > EndAt actual.

•	CloseEarly: si aún activa; establece EndAt = now.

•	SelectOption:

•	Usuario debe ser destinatario.

•	Votación en estado Active (HasStarted y !HasEnded).

•	Usuario no debe haber votado previamente.

•	ConfirmVote:

•	Debe existir voto previo.

•	Vote

•	Confirm: no puede confirmar dos veces (lanza excepción si Confirmed = true).



Nivel de infraestructura (repositorios / data)



•	Consultas aplican filtros (Search, OrderBy) controlados por QueryParam antes de la proyección.

•	Constraints de base (FK y uniqueness) complementan validaciones (un único voto por usuario por votación, destinatario único).

•	Reglas de acceso basadas en rol se validan en handlers (no se exponen directo en DB).



Autorización / rol (validaciones en handlers)



•	CreateVoting / Edit / ExtendEnd / CloseEarly / Recipients / Dashboard: requieren rol ADMIN (HasRoleAsync).

•	SelectVote / ConfirmVote / GetUserVoteStatus: usuario destinatario (validado por lógica y repos).

•	Acceso a detalle: si usuario es destinatario (control externo en capa superior si se refuerza) o creador.



Errores y manejo



•	Excepciones de dominio (InvalidOperationException / ArgumentException) capturadas por handlers → ApiResponse.Failure.

•	Validaciones FluentValidation fallidas → interceptadas por pipeline (ValidationBehavior) y convertidas en errores uniformes.

•	Mensajes consistentes: “Voting not started yet”, “Voting already closed”, “User already voted”, etc.



Resumen de flujo de validación



1\.	Input llega al controlador.

2\.	FluentValidation asegura estructura y formato.

3\.	Handler verifica rol y estado (negocio).

4\.	Dominio refuerza invariantes y lanza excepción si algo viola las reglas.

5\.	Repositorio aplica filtros y persistencia segura.

6\.	Respuesta se retorna en ApiResponse exitoso o con errores.



Cobertura de validaciones clave



•	Temporalidad (StartAt/EndAt) controlada en ValueObject y en ExtendEnd/Edit.

•	Unicidad de voto (SelectOption + constraint).

•	Estados de votación para permitir acciones (Upcoming vs Active vs Closed).

•	Confirmación irreversible.



